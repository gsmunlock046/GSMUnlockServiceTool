using System.Management;
namespace GSMUnlockServiceTool.Services;
public sealed class UsbDeviceInfo { public string Name {get;init;}=""; public string DeviceId {get;init;}=""; public string Status {get;init;}=""; }
public static class DeviceService
{
    public static List<UsbDeviceInfo> GetUsbDevices()
    {
        var items=new List<UsbDeviceInfo>();
        try {
            using var searcher=new ManagementObjectSearcher("SELECT Name, PNPDeviceID, Status FROM Win32_PnPEntity WHERE PNPDeviceID LIKE 'USB%' OR PNPDeviceID LIKE '%VID_%'");
            using var results=searcher.Get();
            foreach(ManagementObject obj in results) {
                var name=obj["Name"]?.ToString()??"Dispositivo USB";
                var id=obj["PNPDeviceID"]?.ToString()??"";
                var status=obj["Status"]?.ToString()??"Desconhecido";
                if(name.Contains("Hub",StringComparison.OrdinalIgnoreCase)||name.Contains("Host Controller",StringComparison.OrdinalIgnoreCase)) continue;
                items.Add(new UsbDeviceInfo{Name=name,DeviceId=id,Status=status});
            }
        } catch(Exception ex) { items.Add(new UsbDeviceInfo{Name="Falha ao consultar inventário USB",DeviceId=ex.Message,Status="Erro"}); }
        return items.OrderBy(x=>x.Name).ToList();
    }
}