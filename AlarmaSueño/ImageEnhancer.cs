using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace AlarmaSueño
{
    /// <summary>
    /// Proporciona métodos para crear versiones mejoradas de imágenes, como logos,
    /// con efectos visuales avanzados como reescalado HiDPI, sombras, biseles y brillos.
    /// </summary>
    public static class ImageEnhancer
    {
        // --- MÉTODO PÚBLICO PRINCIPAL ---

        /// <summary>
        /// Crea las variaciones "normal" y "hover" de un logo con efectos de alta calidad.
        /// </summary>
        /// <param name="originalLogo">La imagen base del logo.</param>
        /// <param name="finalSize">El tamaño final deseado para el logo en la GUI.</param>
        /// <param name="glowColor">El color para el efecto de brillo en el estado "hover".</param>
        /// <returns>Un diccionario con las imágenes "normal" y "hover" procesadas.</returns>
        public static Dictionary<string, Image> CreateLogoVariations(Image originalLogo, Size finalSize, Color glowColor)
        {
            // Paso 1: Renderizar el logo a alta calidad con el tamaño correcto.
            // Para evitar artefactos en los bordes con los efectos, creamos un lienzo un poco más grande.
            int padding = 10;
            Size canvasSize = new Size(finalSize.Width + padding * 2, finalSize.Height + padding * 2);
            Size logoSize = finalSize;

            using (Image highQualityLogo = ResizeImage(originalLogo, logoSize, InterpolationMode.HighQualityBicubic))
            {
                // Paso 2: Crear la versión normal (Sombra + Bisel)
                Image normalImage = CreateImageWithEffects(highQualityLogo, canvasSize, logoSize, padding, null);

                // Paso 3: Crear la versión hover (Brillo + Sombra + Bisel)
                Image hoverImage = CreateImageWithEffects(highQualityLogo, canvasSize, logoSize, padding, glowColor);

                return new Dictionary<string, Image>
                {
                    { "normal", normalImage },
                    { "hover", hoverImage }
                };
            }
        }

        // --- LÓGICA DE CREACIÓN DE EFECTOS ---

        private static Image CreateImageWithEffects(Image logo, Size canvasSize, Size logoSize, int padding, Color? glowColor)
        {
            Bitmap result = new Bitmap(canvasSize.Width, canvasSize.Height);
            using (Graphics g = Graphics.FromImage(result))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                Point logoPosition = new Point(padding, padding);

                // Efecto 1: Brillo (si se especifica un color)
                if (glowColor.HasValue)
                {
                    DrawGlow(g, logo, logoPosition, logoSize, glowColor.Value);
                }

                // Efecto 2: Sombra suave
                DrawDropShadow(g, logo, logoPosition, logoSize);

                // Imagen principal
                g.DrawImage(logo, new Rectangle(logoPosition, logoSize));

                // Efecto 3: Bisel de luz superior (desactivado a petición del usuario)
                // DrawTopBevel(g, new Rectangle(logoPosition, logoSize));
            }
            return result;
        }

        // --- HELPERS PARA EFECTOS INDIVIDUALES ---

        /// <summary>
        /// Dibuja una sombra suave y desplazada.
        /// </summary>
        private static void DrawDropShadow(Graphics g, Image image, Point position, Size size, int offset = 3, int alpha = 80)
        {
            using (var shadowAttributes = new ImageAttributes())
            {
                var matrix = new ColorMatrix(new[]
                {
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, (float)alpha / 255, 0},
                    new[] {0f, 0, 0, 0, 1}
                });
                shadowAttributes.SetColorMatrix(matrix);

                g.DrawImage(image,
                    new Rectangle(position.X + offset, position.Y + offset, size.Width, size.Height),
                    0, 0, image.Width, image.Height,
                    GraphicsUnit.Pixel,
                    shadowAttributes);
            }
        }

        /// <summary>
        /// Dibuja un bisel de luz sutil en la parte superior de la imagen.
        /// </summary>
        private static void DrawTopBevel(Graphics g, Rectangle bounds, int height = 2, int alpha = 40)
        {
            Rectangle bevelRect = new Rectangle(bounds.X, bounds.Y, bounds.Width, height);
            using (var brush = new LinearGradientBrush(bevelRect,
                                                       Color.FromArgb(alpha, Color.White),
                                                       Color.FromArgb(0, Color.White),
                                                       LinearGradientMode.Vertical))
            {
                g.FillRectangle(brush, bevelRect);
            }
        }
        
        /// <summary>
        /// Simula un brillo exterior dibujando la imagen varias veces con transparencia.
        /// </summary>
        private static void DrawGlow(Graphics g, Image image, Point position, Size size, Color glowColor, int layers = 5, int alpha = 150)
        {
            using (var glowAttributes = new ImageAttributes())
            {
                float r = glowColor.R / 255f;
                float gg = glowColor.G / 255f;
                float b = glowColor.B / 255f;
                
                var matrix = new ColorMatrix(new[]
                {
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, 0, 0},
                    new[] {0f, 0, 0, (float)alpha / (255 * layers), 0}, // Reparte la opacidad entre las capas
                    new[] {r, gg, b, 0, 1}
                });
                glowAttributes.SetColorMatrix(matrix);

                for (int i = 0; i < layers; i++)
                {
                     // Dibuja en un área ligeramente más grande para el efecto "glow"
                    var glowRect = new Rectangle(
                        position.X - i, 
                        position.Y - i, 
                        size.Width + (i * 2), 
                        size.Height + (i * 2));

                    g.DrawImage(image, glowRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, glowAttributes);
                }
            }
        }

        // --- HELPER DE REESCALADO ---

        /// <summary>
        /// Redimensiona una imagen a un tamaño específico con un modo de interpolación de alta calidad.
        /// </summary>
        private static Bitmap ResizeImage(Image image, Size size, InterpolationMode quality)
        {
            var destRect = new Rectangle(0, 0, size.Width, size.Height);
            var destImage = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppPArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceOver;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = quality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
    }
}