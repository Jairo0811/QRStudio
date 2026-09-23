<div align="center">

<img src="src/QRStudio.Presentation/Assets/qr-studio-branding.png" alt="QR Studio" width="620">

### QR Studio

Aplicación de escritorio para generar, personalizar, exportar y administrar códigos QR.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/UI-WPF-0C54C2?logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/wpf/)
[![CI](https://github.com/Jairo0811/QRStudio/actions/workflows/ci.yml/badge.svg)](https://github.com/Jairo0811/QRStudio/actions/workflows/ci.yml)
[![Versión](https://img.shields.io/badge/versión-v1.0.0-A56CFF)](CHANGELOG.md)
[![Estado](https://img.shields.io/badge/estado-congelado-64748B)](docs/FINAL_STATUS.md)
[![Licencia](https://img.shields.io/badge/licencia-MIT-22C55E)](LICENSE)

</div>

## Estado del proyecto

**QR Studio v1.0.0 es la versión académica final.** El proyecto queda congelado como archivo de portafolio y no tiene un roadmap activo de nuevas funcionalidades.

La versión final conserva un alcance deliberadamente pequeño: crear QR, personalizarlos, exportarlos y mantener un historial local. Ideas anteriores como lector por cámara, Wi-Fi, vCard, geolocalización, SVG/PDF o sincronización se cerraron como **no planificadas** al decidir conservar el repositorio como tarea académica restaurada.

## Funcionalidades finales

- Generación de códigos QR para texto, sitios web, correo, teléfono y SMS.
- Personalización de color principal, fondo, escala y zona de seguridad.
- Selección del nivel de corrección de errores.
- Vista previa dentro de la aplicación.
- Exportación PNG.
- Historial local persistente en `%LOCALAPPDATA%\QR Studio\history.json`.
- Reutilización, copia y eliminación de configuraciones guardadas.
- Interfaz WPF oscura.
- Icono de aplicación.
- Build y pruebas automatizadas en Windows mediante GitHub Actions.
- Distribución portable y mediante instalador para Windows x64.

## Stack

- C# / .NET 10.
- WPF.
- MVVM con CommunityToolkit.Mvvm.
- Generic Host e inyección de dependencias.
- QRCoder.
- Persistencia JSON local.
- xUnit + Coverlet.
- GitHub Actions.
- Inno Setup para el instalador.

## Arquitectura

```text
QRStudio/
├── src/
│   ├── QRStudio.Domain/
│   ├── QRStudio.Application/
│   ├── QRStudio.Infrastructure/
│   └── QRStudio.Presentation/
├── tests/
│   ├── QRStudio.Application.Tests/
│   └── QRStudio.Infrastructure.Tests/
├── installer/
├── docs/
└── .github/workflows/
```

La solución mantiene las responsabilidades separadas entre Domain, Application, Infrastructure y Presentation. Consulta [ARCHITECTURE.md](docs/ARCHITECTURE.md).

## Ejecución desde código

Requiere Windows 10/11 y .NET 10 SDK.

```bash
git clone https://github.com/Jairo0811/QRStudio.git
cd QRStudio
dotnet restore QRStudio.sln
dotnet build QRStudio.sln
dotnet run --project src/QRStudio.Presentation
```

## Pruebas

```bash
dotnet test QRStudio.sln --configuration Release
```

## Distribución

La versión final genera dos artefactos para Windows x64:

- `QRStudio-v1.0.0-Setup.exe`: instalador por usuario.
- `QRStudio-v1.0.0-win-x64-portable.zip`: publicación portable self-contained.

Los artefactos y sus hashes SHA-256 se producen automáticamente con el workflow de release. Consulta [RELEASE_NOTES.md](RELEASE_NOTES.md).

## Origen académico

Este proyecto nació como una **tarea académica**, no como proyecto final, para la asignatura **Diseño Centrado en el Usuario (SOF-010)** del Instituto Tecnológico de Las Américas (ITLA).

| Información | Detalle |
|---|---|
| Estudiante | Francis Jairo Matías Rosario |
| Matrícula | 2015-2984 |
| Asignatura | Diseño Centrado en el Usuario (SOF-010) |
| Profesor | Juan Martínez López |
| Institución | Instituto Tecnológico de Las Américas (ITLA) |
| Período académico | 2018-C1 |
| Tipo de entrega | Tarea académica |

## Evolución

| Etapa | Implementación |
|---|---|
| GeneradorQR original | WinForms, .NET Framework 4.7.1, una sola pantalla |
| QR Studio v0.1 | Reescritura WPF/.NET 10, MVVM, arquitectura modular y pruebas |
| QR Studio v1.0 | Cierre académico, distribución, metadata, licencia y congelación |

## Mantenimiento

No se planifican nuevas características. El repositorio se conserva como referencia académica y de portafolio. Solo tendría sentido reabrirlo ante una corrección crítica que impida compilar o ejecutar la versión final.

## Licencia

MIT. Consulta [LICENSE](LICENSE).
