using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinClicker
{
    public class TutorialText
    {
        private string data;
        private int fontSize;
        private string innerFontColor;
        private string outerFontColor;
        private int strokeThickness;

        public string Data { get => data; set => data = value; }
        public int FontSize { get => fontSize; set => fontSize = value; }
        public string InnerFontColor { get => innerFontColor; set => innerFontColor = value; }
        public string OuterFontColor { get => outerFontColor; set => outerFontColor = value; }
        public int StrokeThickness { get => strokeThickness; set => strokeThickness = value; }

        public TutorialText() { 
            data = string.Empty;
            fontSize = 16;
            innerFontColor = "White";
            outerFontColor = "Black";
            StrokeThickness = 1;
        }

        public TutorialText(string data, int fontSize, string innerFontColor, string outerFontColor, int strokeThickness) { 
            this.data = data;
            this.fontSize = fontSize;
            this.innerFontColor = innerFontColor;
            this.outerFontColor = outerFontColor;
            this.strokeThickness = strokeThickness;
        }
    }

    public class Tutorial : ObservableObject
    {
        private TutorialText title;
        private IEnumerable<TutorialText> descriptions;

        public TutorialText Title { get => title; set => SetProperty(ref title, value); }
        public IEnumerable<TutorialText> Descriptions { get => descriptions; set => SetProperty(ref descriptions, value); }

        public Tutorial() {
            title = new TutorialText();
            descriptions = new List<TutorialText>() {
                new TutorialText("Empty", 12, "White", "Black", 1),
                new TutorialText("Empty", 24, "White", "Black", 1)
            };
        }

    }
}
