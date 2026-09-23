# Estado final — QR Studio v1.0.0

**Fecha de cierre:** 23 de septiembre de 2026  
**Estado:** congelado / archivo académico  
**Desarrollo futuro:** no planificado

## Criterio de cierre

QR Studio se conserva como una tarea académica restaurada. El objetivo final no es competir como producto comercial, sino dejar una implementación limpia, compilable, documentada y distribuible.

## Incluido en la versión final

- Arquitectura Domain / Application / Infrastructure / Presentation.
- WPF + MVVM sobre .NET 10.
- Generación de QR para texto, web, correo, teléfono y SMS.
- Personalización de diseño y nivel de corrección.
- Exportación PNG.
- Historial local JSON.
- Pruebas automatizadas.
- CI en Windows.
- Versionado 1.0.0 y metadata de ensamblado.
- Icono de aplicación.
- Publicación self-contained win-x64.
- ZIP portable.
- Instalador Inno Setup.
- Checksums SHA-256.
- Licencia MIT.
- Notas de versión.

## Validación automatizada

Los workflows ejecutan restore, build Release, pruebas, publish self-contained para win-x64, creación del ZIP portable, compilación del instalador y cálculo de hashes SHA-256.

La publicación de GitHub Release se ejecuta únicamente al integrar el cierre en `master`.

## Decisiones de alcance

No se implementarán el lector de QR, cámara, Wi-Fi, vCard, geolocalización, calendario, SVG/PDF, plantillas, favoritos, cuentas ni nube.

Las validaciones manuales específicas en Android/iOS y material promocional adicional se retiraron como requisito al congelarse el proyecto como tarea académica. No se documentan como realizadas.

## Política de mantenimiento

El proyecto no recibe nuevas funcionalidades. Solo se justificaría una modificación posterior si fuese necesaria para corregir un fallo crítico de compilación o ejecución.
