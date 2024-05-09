using System;
using System.Windows;
using System.Windows.Media;

namespace CoinClicker
{
    public class BadgeDisplay : FrameworkElement
    {
        private MainWindowViewModel mainWindowViewModel;

        private int upgradeBadgesXOffset = 15;
        private int upgradeBadgesYOffset = 20;
        private int upgradeBadgesYMargin = 15;
        private int maxUpgradeBadgePerColumn = 6;
        private int upgradeBadgeColumnOffset = 15;
        private Point upgradeBadgesSize = new Point(35, 35);
        private int upgradeBadgeFontYOffset = -5;
        private int upgradeBadgeFontXOffset = 0;
        private Brush upgradeBadgeFontInnerBrush = Brushes.White;
        private Brush upgradeBadgeFontOuterBrush = Brushes.Black;
        private int upgradeBadgeFontSize = 12;
        private int upgradeBadgeFontOuterThick = 1;

        private double time;

        public IClickerLogic ClickerLogic { get => mainWindowViewModel.ClickerLogic; }

        public BadgeDisplay(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;

            ClickerLogic.OnUpgradeBuyed += InvalidateVisual;

            CompositionTarget.Rendering += (o, e) => InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            int upGXOff = upgradeBadgesXOffset;
            int upGYOff = upgradeBadgesYOffset;
            int sx = (int)upgradeBadgesSize.X;
            int sy = (int)upgradeBadgesSize.Y;
            Brush ic = upgradeBadgeFontInnerBrush;
            Brush oc = upgradeBadgeFontOuterBrush;
            int fontSize = upgradeBadgeFontSize;
            int outerThick = upgradeBadgeFontOuterThick;

            for (int i = 0; i < ClickerLogic.FullUpgrades.Count; i++)
            {
                int columnIndex = (int)(i / (double)maxUpgradeBadgePerColumn);
                int rowIndex = i % maxUpgradeBadgePerColumn;
                int x = upGXOff + (upgradeBadgeColumnOffset + sx) * columnIndex;
                int y = upGYOff + (upgradeBadgesYMargin * 2 + sy) * rowIndex;
                FullUpgrade fullUpgrade = ClickerLogic.FullUpgrades[i];

                drawingContext.DrawImage(fullUpgrade.Image, new Rect(x, y, sx, sy));

                Point fontPos = new Point(x + sx + upgradeBadgeFontXOffset, y + upgradeBadgeFontYOffset);
                string txt = DoubleFormatter.FormatShort(fullUpgrade.Upgrade.NumberOf);
                CustomFontDrawer.DrawString(drawingContext, txt, fontSize, ic, oc, fontPos, outerThick);
            }
        }
    }
}
