namespace GSMUnlockServiceTool.Services;
public sealed record AndroidDevice(string Serial,string State,string Details);
public static class AndroidService
{
    private static string FindTool(string tool) { var local=Path.Combine(AppContext.BaseDirectory,"platform-tools",tool+".exe"); return File.Exists(local)?local:tool+".exe"; }
    public static Task<CommandResult> RunAdbAsync(string args)=>CommandRunner.RunAsync(FindTool("adb"),args);
    public static Task<CommandResult> RunFastbootAsync(string args)=>CommandRunner.RunAsync(FindTool("fastboot"),args);
    public static async Task<List<AndroidDevice>> GetAdbDevicesAsync()
    {
        var result=await RunAdbAsync("devices -l"); var devices=new List<AndroidDevice>(); if(!result.Success)return devices;
        foreach(var line in result.Output.Split('\n').Skip(1)) { var t=line.Trim(); if(string.IsNullOrWhiteSpace(t)||t.StartsWith("*")||t.StartsWith("List of"))continue; var parts=t.Split(' ',StringSplitOptions.RemoveEmptyEntries); if(parts.Length>=2)devices.Add(new(parts[0],parts[1],parts.Length>2?string.Join(" ",parts.Skip(2)):"")); }
        return devices;
    }
    public static async Task<List<string>> GetFastbootDevicesAsync() { var r=await RunFastbootAsync("devices"); return r.Success?r.Output.Split('\n',StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()).Where(x=>x.Length>0).ToList():new(); }
    public static async Task<string> GetDeviceInfoAsync(string serial)
    {
        var safe=serial.Replace("\"",""); var fields=new[]{("Fabricante","ro.product.manufacturer"),("Marca","ro.product.brand"),("Modelo","ro.product.model"),("Dispositivo","ro.product.device"),("Android","ro.build.version.release"),("Versão SDK","ro.build.version.sdk"),("Build","ro.build.display.id")};
        var output=new List<string>(); foreach(var f in fields) { var r=await RunAdbAsync($"-s \"{safe}\" shell getprop {f.Item2}"); output.Add($"{f.Item1}: {(r.Success?r.Output.Trim():"indisponível")}"); } return string.Join(Environment.NewLine,output);
    }
}