using System.Windows;
using System.Windows.Media;

namespace CoinClicker
{
    public class AnimatedTextDrawer
    {
        private bool drawing;
        private double time;

        public void StartAnimation()
        {
            if (!drawing)
                time = 0;
            drawing = true;
        }

        public void EndAnimation()
        {
            drawing = false;
        }

        public void DrawText(DrawingContext drawingContext, Point position, string text, SolidColorBrush innerBrush, Brush outerBrush, int strokeWeight = 4, int fontSize = 88, double timeStep = 0.1, double rotateIntensity = 2, double rotateSpeed = 3, double sizeIntensity = 5)
        {
            if (drawing)
            {
                time += timeStep;

                RotateTransform rotateTransform = new RotateTransform(Math.Sin(time * rotateIntensity) * rotateSpeed);
                rotateTransform.CenterX = position.X;
                rotateTransform.CenterY = position.Y;

                drawingContext.PushTransform(rotateTransform);

                FormattedText ft = ParticleSystemDrawer.FromString(text);
                double updatedFontSize = fontSize * ((Math.Cos(time) / sizeIntensity) + 1f);


                ft.SetFontSize(updatedFontSize);
                ft.SetForegroundBrush(innerBrush);

                Point newPos = new Point(position.X - ft.Width / 2, position.Y);

                drawingContext.DrawGeometry(null, new Pen(outerBrush, strokeWeight), ft.BuildGeometry(newPos));
                drawingContext.DrawText(ft, newPos);

                drawingContext.Pop();
            }
        }
    }
}
