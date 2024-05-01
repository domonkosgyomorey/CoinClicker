using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace CoinClicker
{
    public class StockPriceChart : FrameworkElement
    {

        private double minValue;
        private double maxValue;
        private double minDate;
        private double maxDate;
        private IStockLogic stockLogic;

        private const double chartXScale = 1.1;
        private const double chartXTransp = 50;
        private const double chartStockShowerCount = 10;
        private const double chartStockShowerXOffset = 10;

        public StockPriceChart(IStockLogic stockLogic)
        {
            this.stockLogic = stockLogic;
            CompositionTarget.Rendering += Loop;
        }

        private void CalculateMinMaxValues()
        {
            if (Dates != null && Prices != null && Prices.Count > 0 && Dates.Count > 0 && Prices.Count == Dates.Count)
            {
                minValue = Prices.Min();
                maxValue = Prices.Max();
                minDate = Dates.Min();
                maxDate = Dates.Max();

                if (stockLogic.SelectedStock.BuyedStockPrice.Count > 0)
                {
                    minValue = Math.Min(minValue, stockLogic.SelectedStock.BuyedStockPrice.Min());
                    maxValue = Math.Max(maxValue, stockLogic.SelectedStock.BuyedStockPrice.Max());
                }
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            CalculateMinMaxValues();

            if (Prices == null || Dates == null || Prices.Count != Dates.Count)
                return;

            double width = ActualWidth;
            double height = ActualHeight;



            if (width == 0 || height == 0)
                return;

            Pen pen;

            double xScale = (maxDate - minDate) != 0 ? width / (maxDate - minDate) : 0;
            double yScale = (maxValue - minValue) != 0 ? height / (maxValue - minValue) : 0;


            for (int i = 0; i <= chartStockShowerCount; i++)
            {
                double y = i * height / chartStockShowerCount;
                double price = maxValue - i * (maxValue - minValue) / chartStockShowerCount;
                string priceText = DoubleFormatter.FormatShort(price);

                drawingContext.DrawLine(new Pen(Brushes.Gray, 1), new Point(0, y), new Point(chartStockShowerXOffset, y));

                FormattedText formattedText = new FormattedText(priceText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 10, Brushes.Yellow);
                drawingContext.DrawText(formattedText, new Point(12, y - 5));
            }

            for (int i = 0; i < Prices.Count - 1; i++)
            {
                double x1 = (Dates[i] - minDate) * xScale / chartXScale + chartXTransp;
                double y1 = -(Prices[i] - minValue) * yScale + height;
                double x2 = (Dates[i + 1] - minDate) * xScale / chartXScale + chartXTransp;
                double y2 = -(Prices[i + 1] - minValue) * yScale + height;

                if (y1 < y2)
                {
                    pen = new Pen(Brushes.Red, 2);
                }
                else
                {
                    pen = new Pen(Brushes.Green, 2);
                }
                drawingContext.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
            }

            if (stockLogic.SelectedStock.BuyedStockPrice.Count > 0)
            {
                foreach (var val in stockLogic.SelectedStock.BuyedStockPrice)
                {
                    double yBuyPrice = (val - minValue) * yScale;
                    drawingContext.DrawLine(new Pen(Brushes.Gray, 1) { DashStyle = DashStyles.Dash }, new Point(0, height - yBuyPrice), new Point(width, height - yBuyPrice));

                    double lastPrice = Prices[Prices.Count - 1];
                    string changeSymbol = lastPrice > val ? "+" : "-";
                    Brush brush = changeSymbol == "+" ? Brushes.Green : Brushes.Red;
                    string changeText = $" ({changeSymbol})";

                    FormattedText formattedText = new FormattedText(changeText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface("Arial"), 12, brush);
                    double textX = (Dates[Dates.Count - 1] - minDate) * xScale;
                    drawingContext.DrawText(formattedText, new Point(textX + 5, height - yBuyPrice));
                }
            }

        }

        private CircularBuffer<double> Prices
        {
            get => stockLogic.SelectedStock.Prices;
        }

        private CircularBuffer<double> Dates
        {
            get => stockLogic.SelectedStock.Dates;
        }

        public void Loop(object sender, EventArgs args)
        {
            InvalidateVisual();
        }
    }
}
