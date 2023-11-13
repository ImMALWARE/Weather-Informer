#define MyAppName "Weather Informer"
#define MyAppVersion "1.0"
#define MyAppPublisher "MALWARE"
#define MyAppURL "https://malw.ru"
#define MyAppExeName "Weather Informer.exe"

[Setup]
AppId={{7BAC49C6-748B-442F-B295-04C936603754}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\MalwWeatherInformer
DisableProgramGroupPage=yes
PrivilegesRequiredOverridesAllowed=dialog
OutputDir=C:\Users\malw\Desktop
OutputBaseFilename=Weather Informer Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=C:\Users\malw\Documents\Weather-Informer\app.ico

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"
Name: "russian"; MessagesFile: "compiler:Languages\Russian.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Dirs]
Name: "{app}\res"
Name: "{app}\x64"
Name: "{app}\x86"

[Files]
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\EntityFramework.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\EntityFramework.SqlServer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\Microsoft.Toolkit.Uwp.Notifications.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\Newtonsoft.Json.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\System.Data.SQLite.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\System.Data.SQLite.EF6.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\System.Data.SQLite.Linq.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\System.ValueTuple.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\res\*"; DestDir: "{app}\res"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\x64\*"; DestDir: "{app}\x64"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "C:\Users\malw\Documents\Weather-Informer\bin\Debug\x86\*"; DestDir: "{app}\x86"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

