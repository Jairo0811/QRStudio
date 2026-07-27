# GeneradorQR original

El repositorio comenzó como una tarea académica de generación de códigos QR.

## Estado encontrado

- Windows Forms.
- .NET Framework 4.7.1.
- Una única clase `Form1` con lógica de interfaz, generación y guardado.
- QrCode.Net 0.4.0 y QRCoder 1.3.2.
- Referencia absoluta a un `zxing.dll` almacenado fuera del repositorio.
- Exportación limitada a PNG.
- Sin pruebas ni documentación funcional.

## Decisión

QR Studio es una reconstrucción, no una migración progresiva. El código original fue retirado de la rama nueva porque su estructura no aporta una base mantenible. Continúa disponible en el historial de Git y en el commit inicial del repositorio.
