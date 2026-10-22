#define MyAppName "GSM Cleaner Remover Tool"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "GSM Unlock Service"
#define MyAppExeName "GSMUnlockServiceTool.exe"

[Setup]
AppId={{8E14D87D-8B0C-4F0A-8D22-1B6A57C941A1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\GSM Cleaner Remover Tool
DefaultGroupName={#MyAppName}
OutputDir=artifacts\installer
OutputBaseFilename=GSM-Cleaner-Remover-Setup-x64
Compression=lzma
SolidCompression=yes
WizardStyle=modern
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
UninstallDisplayIcon={app}\{#MyAppExeName}

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\BrazilianPortuguese.isl"
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na área de trabalho"; GroupDescription: "Atalhos adicionais:"; Flags: unchecked

[Files]
Source: "artifacts\app\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "Abrir {#MyAppName}"; Flags: postinstall nowait skipifsilent
