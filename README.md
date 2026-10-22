# GSM Unlock Service Tool

Protótipo local para Windows 10/11 x64, C# / .NET 8 Windows Forms.

## Funções
- Inventário USB / Plug and Play via WMI.
- Listagem de dispositivos ADB e Fastboot.
- Leitura de fabricante, marca, modelo, dispositivo, versão Android, SDK e build usando getprop.
- Consulta preliminar de compatibilidade por fabricante/modelo/modo.

## Como usar
O executável será construído pelo GitHub Actions. Para executar localmente com SDK .NET 8 instalado:

```powershell
dotnet restore
dotnet run
```

ADB e Fastboot requerem Android SDK Platform-Tools oficiais: https://developer.android.com/tools/releases/platform-tools . Copie os executáveis e as DLLs relacionadas para a pasta `platform-tools` ao lado do programa, ou configure PATH.

## Publicação local
```powershell
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
```

## Limites
Ferramenta de diagnóstico de leitura. Não realiza FRP bypass, EDL, flashing, apagamento de dados ou desbloqueio de bootloader. Compatibilidade é apenas orientativa. Use apenas em aparelhos próprios ou com autorização do cliente.