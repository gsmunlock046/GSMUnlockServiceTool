namespace GSMUnlockServiceTool.Services;
public static class CompatibilityService
{
 public static string Check(string manufacturer,string model,string mode)
 {
  if(string.IsNullOrWhiteSpace(manufacturer)||string.IsNullOrWhiteSpace(model)) return "Informe fabricante e modelo para consultar.";
  var m=manufacturer.Trim().ToLowerInvariant(); var x=m.Contains("xiaomi")||m.Contains("redmi")||m.Contains("poco");
  var details=mode switch { "ADB"=>"ADB: requer Android iniciado, Depuração USB ativada e computador autorizado.", "Fastboot"=>"Fastboot: requer modo Fastboot ativo e driver compatível.", _=>"USB: requer cabo de dados, porta funcional e driver compatível."};
  return $"Fabricante: {manufacturer.Trim()}\r\nModelo: {model.Trim()}\r\nFamília Xiaomi/Redmi/POCO: {(x?"possível":"não identificada")}\r\n{details}\r\n\r\nConsulta orientativa; não confirma compatibilidade com desbloqueios. Operações protegidas podem exigir autorização do fabricante.";
 }
}