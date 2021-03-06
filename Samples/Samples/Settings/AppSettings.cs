﻿using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Shiny;

namespace Samples.Settings
{
    public class AppSettings : ReactiveObject
    {
        public AppSettings()
        {
            //this.WhenAnyProperty(x => x.IsChecked).Subscribe(_ => this.LastUpdated = DateTime.Now);
            //this.WhenAnyProperty(x => x.YourText).Subscribe(_ => this.LastUpdated = DateTime.Now);
            this.WhenAnyValue(
                    x => x.IsChecked,
                    x => x.YourText
                )
                .Skip(1)
                .Subscribe(_ =>
                    this.LastUpdated = DateTime.Now
                );
        }


        [Reactive] public bool IsChecked { get; set; }
        [Reactive] public string YourText { get; set; }
        [Reactive] public DateTime? LastUpdated { get; set; }
    }
}
