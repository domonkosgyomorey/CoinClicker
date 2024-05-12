using System.Globalization;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Media;

namespace CoinClicker
{
    public class ParticleSystemDrawer
    {

        public static void DrawDouble(ParticleSystem<double> particleSystem, DrawingContext drawingContext, int r, int g, int b)
        {
            if (particleSystem.Particles.Count == 0) return;
            FormattedText str;
            Pen pen;
            foreach (var particle in particleSystem.Particles)
            {
                str = FromString(DoubleFormatter.FormatShort(particle.Item));
                str.SetForegroundBrush(new SolidColorBrush(Color.FromArgb((byte)((particle.Lives / (float)particleSystem.Lives) * 255f), (byte)r, (byte)g, (byte)b)));
                pen = new Pen(new SolidColorBrush(Color.FromArgb((byte)((particle.Lives / (float)particleSystem.Lives) * 255f), 0, 0, 0)), 2);
                Point pos = new Point(particle.Position.X, particle.Position.Y);

                drawingContext.DrawGeometry(null, pen, str.BuildGeometry(pos));
                drawingContext.DrawText(str, pos);
            }
        }

        public static FormattedText FromString(string text)
        {
            return new FormattedText(text, CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight, new Typeface("Raavi"), 28, Brushes.Black);
        }
    }
}
