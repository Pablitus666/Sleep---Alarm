## 🚀 AlarmaSueño — v2.0.0

🎉 **Segunda versión mayor — rediseño arquitectónico y funcional**

**AlarmaSueño v2.0.0** representa una **evolución significativa** del proyecto. No se trata de una mejora incremental, sino de un **rediseño profundo** orientado a **robustez, persistencia de estado, integración real con Windows y una experiencia de usuario confiable**.

Esta versión consolida a AlarmaSueño como una **aplicación de escritorio madura**, con comportamiento consistente incluso ante reinicios del sistema o cierres inesperados.

---

## ✨ Novedades y mejoras clave

### ⏰ Gestión avanzada de alarmas

* Ejecución garantizada mediante el **Programador de Tareas de Windows**
* La alarma se activa incluso si la aplicación no está abierta
* Eliminación de la dependencia exclusiva de timers en memoria

### 💤 Snooze persistente

* La posposición de la alarma **sobrevive cierres de la aplicación**
* El estado de snooze se restaura automáticamente
* La interfaz se bloquea mientras el snooze está activo

### 🔒 Bloqueo real de la alarma

* Bloqueo explícito de la configuración
* El estado de bloqueo persiste tras reinicios
* Prevención efectiva de cambios accidentales

### 🖥️ Integración completa con Windows

* Icono en la **bandeja del sistema (Tray Icon)**
* Restauración de la ventana desde el tray
* **Ejecución de una sola instancia** de la aplicación
* Comunicación entre instancias mediante mensajería de Windows

### 🎨 Experiencia de usuario mejorada

* Interfaz dinámica según el estado de la alarma
* Vistas diferenciadas (configuración / alarma activa)
* Logotipo con efectos visuales
* Diálogos personalizados y consistentes

---

## 🧱 Arquitectura y calidad técnica

* ✔️ Aplicación real de **Clean Architecture**
* ✔️ Separación estricta de responsabilidades (UI / Core / Infraestructura)
* ✔️ Uso extensivo de **interfaces**
* ✔️ **Inyección de Dependencias** con `Microsoft.Extensions.DependencyInjection`
* ✔️ Persistencia centralizada del estado de la aplicación
* ✔️ Integración controlada con el sistema operativo
* ✔️ Código preparado para extensiones futuras sin reescritura
* ✔️ Base sólida para una migración futura a **WPF / .NET MAUI**

> ℹ️ En esta versión se priorizó estabilidad, arquitectura y confiabilidad por encima de nuevas funcionalidades superficiales.

---

## 🖥️ Requisitos del sistema

* ✔️ Windows 10 / Windows 11
* ✔️ .NET Desktop Runtime 6.0 o superior
* ✔️ Permisos de usuario estándar (no requiere administrador)

---

## 📦 Distribución

Este release se distribuye como:

* 🧾 **Ejecutable (.exe)** listo para usar
* 🚫 No requiere instalación
* ▶️ Descarga directa desde la sección **Releases**

---

## 📌 Estado del proyecto

* ✔️ Estable
* ✔️ Arquitectura madura
* ✔️ Persistencia de estado confiable
* ✔️ Integración nativa con Windows
* ✔️ Apto para uso diario

---

## 🔮 Roadmap

* 🔄 Exportación y respaldo de configuraciones
* 🎨 Nuevos temas visuales
* 🔊 Soporte para múltiples sonidos
* 📊 Registro de eventos e historial de alarmas
* 🚀 Migración opcional a WPF / .NET MAUI

---

## 📝 Notas finales

**AlarmaSueño v2.0.0** marca el paso de un proyecto funcional a una **aplicación de escritorio profesional**, con estado persistente, arquitectura limpia e integración profunda con Windows.

Este release consolida una base técnica sólida, adecuada tanto para **uso real diario** como para **proyecto de portafolio avanzado en C# / .NET**.
