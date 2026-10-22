using System.Text;
namespace GSMUnlockServiceTool.Services;
public static class IPhoneService
{
    static string Tool(string name) {
        var p=Path.Combine(AppContext.BaseDirectory,"tools","libimobiledevice",name+".exe");
        return File.Exists(p)?p:name+".exe";
    }
    static Task<CommandResult> Run(string tool,string args,int timeout=15000)=>CommandRunner.RunAsync(Tool(tool),args,timeout);
    public static async Task<List<string>> ListDevicesAsync() {
        var r=await Run("idevice_id","-l");
        return r.Success?r.Output.Split(new[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).Where(x=>x.Length>0).ToList():new();
    }
    public static async Task<string> ReadFullReportAsync(string udid) {
        if(string.IsNullOrWhiteSpace(udid)||udid.Any(char.IsWhiteSpace)||udid.Contains('"')) return "Identificador do iPhone inválido.";
        var sb=new StringBuilder();
        sb.AppendLine("=== RELATÓRIO DE IPHONE (USB) ===");
        sb.AppendLine("UDID: "+udid);
        sb.AppendLine("Data da leitura: "+DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sb.AppendLine();
        var keys=new (string Label,string Key)[] {
          ("Nome do aparelho","DeviceName"),("Modelo comercial (se fornecido pelo iOS)","MarketingName"),
          ("Tipo/modelo Apple","ProductType"),("Identificador do hardware","HardwareModel"),
          ("Versão iOS","ProductVersion"),("Build iOS","BuildVersion"),("Número de série","SerialNumber"),
          ("IMEI","InternationalMobileEquipmentIdentity"),("IMEI secundário","InternationalMobileEquipmentIdentity2"),
          ("MEID","MobileEquipmentIdentifier"),("Modem firmware","BasebandVersion"),
          ("Wi-Fi MAC","WiFiAddress"),("Bluetooth MAC","BluetoothAddress"),
          ("Capacidade total (bytes)","TotalDiskCapacity"),("Espaço livre (bytes)","TotalDataCapacity"),
          ("Região/país","国别")
        };
        foreach(var item in keys) {
          if(item.Key=="国别") continue;
          var r=await Run("ideviceinfo",$"-u {udid} -k {item.Key}");
          var value=r.Success?r.Output.Trim():"";
          if(string.IsNullOrWhiteSpace(value)) value="Não fornecido por este aparelho/iOS ou indisponível";
          sb.AppendLine($"{item.Label}: {value}");
        }
        sb.AppendLine();
        sb.AppendLine("=== BATERIA / DIAGNÓSTICO ===");
        var battery=await Run("idevicediagnostics",$"-u {udid} diagnostics",20000);
        if(battery.Success && !string.IsNullOrWhiteSpace(battery.Output)) sb.AppendLine(battery.Output.Trim());
        else sb.AppendLine("Dados de bateria não disponíveis pela ferramenta/versão do iOS. " + battery.Error);
        sb.AppendLine();
        sb.AppendLine("Observação: iOS e modelo podem ocultar IMEI, capacidade/saúde da bateria ou outros campos. Campos ausentes não significam defeito.");
        return sb.ToString();
    }
}