# ⏰ AlarmaSueño – Alarma Motivacional para Windows (C#)

Aplicación de escritorio desarrollada en **C# con Windows Forms**, orientada a mejorar la rutina de sueño mediante una **alarma inteligente con frases motivacionales**, ejecución en segundo plano, integración con la bandeja del sistema y una arquitectura moderna preparada para evolución futura.

El proyecto sigue principios **Clean Architecture**, **SOLID**, **Inyección de Dependencias (DI)** y cuenta con **pruebas unitarias con Moq**, garantizando mantenibilidad, testabilidad y escalabilidad.

> 📦 **Distribución:** el proyecto se entrega como **archivo ejecutable (.exe)** listo para usar, disponible en la sección **Releases** del repositorio.

![Social Preview](images/Preview.png)

---

## 🚀 Badges

![Platform](https://img.shields.io/badge/platform-Windows-0078D6?logo=windows\&logoColor=white)
![Language](https://img.shields.io/badge/language-C%23-239120?logo=csharp\&logoColor=white)
![Framework](https://img.shields.io/badge/.NET-WinForms-512BD4?logo=dotnet\&logoColor=white)
![Architecture](https://img.shields.io/badge/architecture-Clean%20Architecture-brightgreen)
![SOLID](https://img.shields.io/badge/principles-SOLID-blueviolet)
![DI](https://img.shields.io/badge/dependency%20injection-Implemented-blue)
![Tests](https://img.shields.io/badge/tests-Moq%20%7C%20Unit%20Tests-yellowgreen)
![Status](https://img.shields.io/badge/status-Stable-success)

---

## 📚 Tabla de Contenidos

* [✨ Características](#-características)
* [🏗️ Arquitectura](#️-arquitectura)
* [🧪 Pruebas Unitarias](#-pruebas-unitarias)
* [📸 Capturas de Pantalla](#-capturas-de-pantalla)
* [📋 Requisitos](#-requisitos)
* [🚀 Uso](#-uso)
* [📦 Instalación (Release)](#-instalación-release)
* [🛡️ Seguridad y Buenas Prácticas](#️-seguridad-y-buenas-prácticas)
* [🤝 Contribuciones](#-contribuciones)
* [👤 Autor](#-autor)
* [📄 Licencia](#-licencia)

---

## ✨ Características

* ⏰ **Alarma programable** con ejecución automática
* 💬 **Frases motivacionales dinámicas** al activarse la alarma
* 🔔 **Reproducción de audio** integrada
* 💤 **Función posponer (Snooze)** configurable
* 🔒 **Bloqueo de configuración** para evitar cambios accidentales
* 🚀 **Inicio automático con Windows** (opcional)
* 🌐 **Soporte multi‑idioma (I18n)**
* 🖥️ **Ejecución en segundo plano** con icono en la bandeja del sistema
* 🎨 **Interfaz moderna** con botones e imágenes personalizadas

---

## 🏗️ Arquitectura

El proyecto está organizado siguiendo **Clean Architecture**, separando responsabilidades:

```
AlarmaSueño
│
├── AlarmaSueño.Core        → Lógica de negocio
│   ├── AlarmManager
│   ├── SettingsManager
│   ├── PhraseProvider
│   └── Interfaces (IAlarmManager, IAudioPlayer, etc.)
│
├── AlarmaSueño.UI          → Windows Forms (UI)
│   ├── MainForm
│   ├── Dialogs
│   └── Custom Controls
│
├── AlarmaSueño.Tests       → Pruebas unitarias (Moq)
│
└── Assets / Resources      → Imágenes, iconos, audio
```

✔️ Inyección de dependencias aplicada
✔️ Código desacoplado
✔️ Preparado para migración futura a **WPF / MAUI**

---

## 🧪 Pruebas Unitarias

El proyecto incluye pruebas unitarias utilizando **Moq** para validar la lógica crítica:

* `AlarmManager`
* `SettingsManager`
* `PhraseProvider`

✔️ Todas las pruebas se ejecutan correctamente
✔️ Sin dependencias directas de UI
✔️ Enfoque en confiabilidad y regresión

---

## 📸 Capturas de Pantalla

![Pantalla Principal](images/screenshot.png)

---

## 📋 Requisitos

* **Sistema Operativo:** Windows 10 u 11
* **.NET Runtime:**

  * Para ejecutar el `.exe` **no es necesario instalar Visual Studio**
  * Puede requerir **.NET Desktop Runtime 6.0 o superior** si no está presente

---

## 🚀 Uso

1. Ejecuta `AlarmaSueño.exe`
2. Configura la hora de la alarma
3. (Opcional) Activa inicio con Windows
4. Minimiza la aplicación (queda en la bandeja del sistema)
5. Al activarse la alarma:

   * Se reproduce el audio
   * Se muestra una frase motivacional
   * Puedes **cerrar** o **posponer**

---

## 📦 Instalación (Release)

1. Ve a la **sección Releases** del repositorio:
   👉 [https://github.com/Pablitus666/AlarmaSueño/releases](https://github.com/Pablitus666/AlarmaSueño/releases)
2. Descarga el archivo:

   * `AlarmaSueño.exe`
3. Coloca el archivo en cualquier carpeta de tu PC
4. Ejecuta el `.exe`

📌 **No requiere instalación ni configuración adicional**

---

## 🛡️ Seguridad y Buenas Prácticas

* Manejo seguro de excepciones con logging
* Liberación correcta de recursos (`Dispose`)
* Separación estricta de capas
* Código preparado para pruebas y refactorización

---

## 🤝 Contribuciones

Las contribuciones son bienvenidas:

* Fork del repositorio
* Crear rama feature / fix
* Pull Request documentado

---

## 👤 Autor

**Pablo Téllez A.**
Tarija – Bolivia 🇧🇴

---

## 📄 Licencia

Este proyecto está licenciado bajo la
**GNU General Public License v3.0 (GPLv3)**

Consulta el archivo [LICENSE](LICENSE) para más información.
Más detalles en: [https://www.gnu.org/licenses/gpl-3.0.html](https://www.gnu.org/licenses/gpl-3.0.html)
