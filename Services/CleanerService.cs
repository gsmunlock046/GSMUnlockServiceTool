using System.Text.RegularExpressions;

namespace GSMUnlockServiceTool.Services;

public sealed record CleanerPackage(string PackageId, string Source);

public static class CleanerService
{
    private static readonly Regex PackagePattern = new(@"^[A-Za-z0-9_]+(?:\\.[A-Za-z0-9_]+)+$", RegexOptions.Compiled);

    public static async Task<List<CleanerPackage>> FindCleanerPackagesAsync(string serial)
    {
        if (string.IsNullOrWhiteSpace(serial)) throw new ArgumentException("Selecione um aparelho ADB autorizado.");
        var safeSerial = serial.Trim();
        if (safeSerial.Any(char.IsWhiteSpace) || safeSerial.Contains('"')) throw new ArgumentException("Identificador do aparelho inválido.");

        var result = await AndroidService.RunAdbAsync($"-s \"{safeSerial}\" shell pm list packages");
        if (!result.Success) throw new InvalidOperationException(string.IsNullOrWhiteSpace(result.Error) ? "Não foi possível consultar os pacotes." : result.Error);

        return result.Output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim().StartsWith("package:", StringComparison.OrdinalIgnoreCase) ? line.Trim()[8..].Trim() : "")
            .Where(id => id.Length > 0 && id.Contains("cleaner", StringComparison.OrdinalIgnoreCase) && PackagePattern.IsMatch(id))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
            .Select(id => new CleanerPackage(id, "ADB"))
            .ToList();
    }

    public static async Task<string> RemoveForCurrentUserAsync(string serial, string packageId)
    {
        if (string.IsNullOrWhiteSpace(serial) || serial.Any(char.IsWhiteSpace) || serial.Contains('"'))
            throw new ArgumentException("Identificador do aparelho inválido.");
        if (!PackagePattern.IsMatch(packageId) || !packageId.Contains("cleaner", StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("Por segurança, só é permitida a remoção de pacotes cujo identificador contenha 'cleaner'.");

        var result = await AndroidService.RunAdbAsync($"-s \"{serial}\" shell pm uninstall --user 0 {packageId}");
        var output = (result.Output + " " + result.Error).Trim();
        if (output.Contains("Success", StringComparison.OrdinalIgnoreCase))
            return $"Remoção concluída para o usuário atual: {packageId}.";
        return $"Não foi possível remover {packageId}. Resposta do aparelho: {output}";
    }
}
