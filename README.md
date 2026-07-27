<div align="center">

<img src="src/QRStudio.Presentation/Assets/qr-studio-branding.png" alt="QR Studio" width="620">

# QR Studio

### Crea · Personaliza · Comparte

Aplicación de escritorio para generar, personalizar, exportar y administrar códigos QR.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![WPF](https://img.shields.io/badge/UI-WPF-0C54C2?logo=windows&logoColor=white)](https://learn.microsoft.com/dotnet/desktop/wpf/)
[![Arquitectura](https://img.shields.io/badge/arquitectura-MVVM-15C8FF)](docs/ARCHITECTURE.md)
[![CI](https://github.com/Jairo0811/QRStudio/actions/workflows/ci.yml/badge.svg)](https://github.com/Jairo0811/QRStudio/actions/workflows/ci.yml)
[![Versión](https://img.shields.io/badge/versión-v0.1.0-A56CFF)](CHANGELOG.md)
[![Estado](https://img.shields.io/badge/estado-en%20desarrollo-22C55E)](docs/ROADMAP.md)

</div>

## Descripción

**QR Studio** es una aplicación de escritorio para Windows que permite crear, personalizar, exportar y administrar códigos QR mediante una interfaz moderna construida con WPF y arquitectura MVVM.

El proyecto nació a partir de una tarea académica llamada **GeneradorQR**, posteriormente reescrita desde cero con .NET 10, arquitectura modular, persistencia local y pruebas automatizadas.

## Funcionalidades

### Funcionalidades disponibles

- Generación de códigos QR para texto, sitios web, correo, teléfono y SMS.
- Personalización de color principal, fondo, escala y zona de seguridad.
- Selección del nivel de corrección de errores.
- Vista previa nítida dentro de la aplicación.
- Exportación en formato PNG.
- Historial local persistente.
- Reutilización, copia y eliminación de configuraciones guardadas.
- Interfaz oscura adaptable a diferentes tamaños de ventana.

### En desarrollo

- Lectura de QR desde imagen, cámara o portapapeles.
- Contenidos Wi-Fi, vCard, ubicación y eventos.
- Exportación SVG y PDF.
- Logotipo central con validación de legibilidad.
- Plantillas y favoritos.
- Empaquetado MSIX e instalador firmado.

Consulta el [roadmap completo](docs/ROADMAP.md).

## Stack tecnológico

### Lenguaje y plataforma

[![My Skills](https://skillicons.dev/icons?i=cs,dotnet,windows)](https://skillicons.dev)

- C# con tipos anulables y analizadores habilitados.
- .NET 10.
- Windows Presentation Foundation.

### Arquitectura y librerías

- MVVM con CommunityToolkit.Mvvm.
- Generic Host e inyección de dependencias.
- QRCoder para renderizado PNG.
- Persistencia JSON en `%LOCALAPPDATA%\QR Studio\history.json`.
- Arquitectura en capas: Domain, Application, Infrastructure y Presentation.

### Pruebas y herramientas

[![My Skills](https://skillicons.dev/icons?i=visualstudio,github,git)](https://skillicons.dev)

- xUnit.
- Microsoft.NET.Test.Sdk.
- Coverlet Collector.
- GitHub Actions.
- Central Package Management.

## Arquitectura

```mermaid
flowchart LR
    UI["Presentation<br>WPF + MVVM"] --> APP["Application<br>casos de uso"]
    APP --> DOM["Domain<br>modelos y reglas"]
    INF["Infrastructure<br>QR + JSON + archivos"] --> APP
    UI --> INF
```

Las dependencias apuntan hacia el núcleo. La interfaz no conoce detalles de QRCoder ni del formato físico del historial. Consulta [ARCHITECTURE.md](docs/ARCHITECTURE.md) para las decisiones y límites de cada capa.

## Estructura

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
├── docs/
├── .github/workflows/
└── QRStudio.sln
```

## Requisitos

- Windows 10 o Windows 11.
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
- Visual Studio con la carga de trabajo **Desarrollo de escritorio de .NET**, o la CLI de .NET.

## Ejecución local

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

Las pruebas cubren el formateo de contenido, la orquestación del caso de uso, la generación de PNG y la persistencia del historial.

## Origen académico

Este proyecto nació como una **tarea académica**, no como proyecto final, para la asignatura **Diseño Centrado en el Usuario (SOF-010)** del Instituto Tecnológico de Las Américas (ITLA).

| Información | Detalle |
|---|---|
| 👨‍🎓 Estudiante | Francis Jairo Matías Rosario |
| 🆔 Matrícula | 2015-2984 |
| 📖 Asignatura | Diseño Centrado en el Usuario (SOF-010) |
| 👨‍🏫 Profesor | Juan Martínez López |
| 🏫 Institución | Instituto Tecnológico de Las Américas (ITLA) |
| 📅 Período académico | 2018-C1 |
| 📝 Tipo de entrega | Tarea académica |

## Evolución del proyecto

| Etapa | Implementación |
|---|---|
| GeneradorQR original | WinForms, .NET Framework 4.7.1, una sola pantalla |
| QR Studio | WPF, .NET 10, MVVM, arquitectura modular y pruebas |

La reconstrucción reemplaza por completo la implementación original, manteniendo su historial en Git y estableciendo una base limpia para la evolución del producto.

## Autor

Desarrollado por **[Jairo Matías](https://github.com/Jairo0811)**  
Desarrollador de software · República Dominicana

## Licencia

La licencia pública se definirá antes de la primera versión estable.
