using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace CoinClicker
{
    public class CustomFontDrawer
    {
        public CustomFontDrawer() { }

        private static FormattedText FormattedTextBuilder(string text, int fontSize, Brush color) {
            return new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Raavi"), fontSize, color);
        }

        public static void DrawString(DrawingContext drawingContext, string text, int fontSize, Brush innerColor, Brush outerColor, Point point, double outerThickness) {
            FormattedText ft = FormattedTextBuilder(text, fontSize, innerColor);
            drawingContext.DrawGeometry(null, new Pen(outerColor, outerThickness), ft.BuildGeometry(point));
            drawingContext.DrawText(ft, point);
        }
    }
}
