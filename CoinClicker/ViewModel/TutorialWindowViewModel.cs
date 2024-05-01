using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinClicker
{
    public class TutorialWindowViewModel : ObservableRecipient
    {
        private Tutorial tutorial;

        public Tutorial Tutorial { get => tutorial; set => tutorial = value; }


        public TutorialWindowViewModel(Tutorial tutorial)
        {
            this.tutorial = tutorial;
        }
    }
}
