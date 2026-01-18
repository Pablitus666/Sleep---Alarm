using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace AlarmaSueño
{
    public static class ResourceLoader
    {
        private static readonly Assembly _assembly = Assembly.GetExecutingAssembly();
        
        // Caché para las imágenes
        private static readonly Dictionary<string, Image> ImageCache = new Dictionary<string, Image>();
        private static readonly object CacheLock = new object();

        public static Image? LoadImage(string resourceName)
        {
            lock (CacheLock)
            {
                // 1. Verificar si la imagen ya está en la caché
                if (ImageCache.ContainsKey(resourceName))
                {
                    return ImageCache[resourceName];
                }

                // 2. Si no está, cargarla desde los recursos
                string fullResourceName = $"AlarmaSueño.images.{resourceName}";
                using (Stream? stream = _assembly.GetManifestResourceStream(fullResourceName))
                {
                    if (stream != null)
                    {
                        MemoryStream memoryStream = new MemoryStream();
                        stream.CopyTo(memoryStream);
                        memoryStream.Position = 0; // Resetear la posición para que Image.FromStream pueda leer desde el principio

                        Image? originalImage = null;
                        try
                        {
                            originalImage = Image.FromStream(memoryStream);
                            if (originalImage != null)
                            {
                                Image clonedImage = new Bitmap(originalImage);
                                
                                // 3. Añadir la imagen clonada a la caché
                                ImageCache[resourceName] = clonedImage;
                                
                                return clonedImage;
                            }
                        }
                        finally
                        {
                            originalImage?.Dispose(); // Disponer de la imagen intermedia
                            memoryStream.Dispose(); // Disponer del MemoryStream
                        }
                    }
                }
                return null;
            }
        }

        public static Icon? LoadIcon(string resourceName)
        {
            // El cache para iconos es menos crítico ya que usualmente se carga una sola vez,
            // pero se podría implementar un sistema similar si fuera necesario.
            string fullResourceName = $"AlarmaSueño.images.{resourceName}"; 

            using (Stream? stream = _assembly.GetManifestResourceStream(fullResourceName))
            {
                if (stream != null)
                {
                    return new Icon(stream);
                }
            }
            return null;
        }
    }
}
