#ifndef AppVersion
  #define AppVersion "1.0.0"
#endif

#ifndef PublishDir
  #define PublishDir "..\artifacts\publish"
#endif

#ifndef OutputDir
  #define OutputDir "..\artifacts\dist"
#endif

#define AppName "QR Studio"
#define AppExeName "QRStudio.Presentation.exe"

[Setup]
AppId={{F9B5157A-7D59-4A47-A88D-6D63F51B4424}
AppName={#AppName}
AppVersion={#AppVersion}
AppPublisher=Jairo Matías
AppPublisherURL=https://github.com/Jairo0811/QRStudio
DefaultDirName={localappdata}\Programs\QR Studio
DefaultGroupName=QR Studio
DisableProgramGroupPage=yes
PrivilegesRequired=lowest
OutputDir={#OutputDir}
OutputBaseFilename=QRStudio-v{#AppVersion}-Setup
SetupIconFile=..\src\QRStudio.Presentation\Assets\qr-studio.ico
UninstallDisplayIcon={app}\{#AppExeName}
Compression=lzma2
SolidCompression=yes
WizardStyle=modern
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible
VersionInfoVersion={#AppVersion}
VersionInfoProductName={#AppName}
VersionInfoCompany=Jairo Matías

[Files]
Source: "{#PublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{autoprograms}\QR Studio"; Filename: "{app}\{#AppExeName}"
Name: "{autodesktop}\QR Studio"; Filename: "{app}\{#AppExeName}"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Crear un acceso directo en el escritorio"; GroupDescription: "Accesos directos adicionales:"; Flags: unchecked

[Run]
Filename: "{app}\{#AppExeName}"; Description: "Abrir QR Studio"; Flags: nowait postinstall skipifsilent
