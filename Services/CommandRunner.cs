using System.Diagnostics;
namespace GSMUnlockServiceTool.Services;
public sealed record CommandResult(int ExitCode, string Output, string Error)
{
    public bool Success => ExitCode == 0;
}
public static class CommandRunner
{
    public static async Task<CommandResult> RunAsync(string fileName, string arguments, int timeoutMs = 12000)
    {
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo { FileName=fileName, Arguments=arguments, UseShellExecute=false, RedirectStandardOutput=true, RedirectStandardError=true, CreateNoWindow=true };
        try {
            process.Start();
            var stdout=process.StandardOutput.ReadToEndAsync();
            var stderr=process.StandardError.ReadToEndAsync();
            using var cts=new CancellationTokenSource(timeoutMs);
            try { await process.WaitForExitAsync(cts.Token); }
            catch(OperationCanceledException) { try { process.Kill(true); } catch {} return new(-1, await stdout, "Tempo limite excedido."); }
            return new(process.ExitCode, await stdout, await stderr);
        } catch(Exception ex) { return new(-1,"",ex.Message); }
    }
}