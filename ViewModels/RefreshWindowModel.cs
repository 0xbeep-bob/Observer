using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Observer.EFModels;
using Observer.Models;
using Observer.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Xml;
using System.Globalization;
using System.Diagnostics;
using System.Threading;

namespace Observer.ViewModels
{
    class RefreshWindowModel : MainWindowModel
    {
        public string Rezult { get; set; } = "Ожидание результата,\nобновление займет около минуты...";
        public bool CheckRun { get; set; } = true;
        public bool CheckEnd { get; set; } = false;

        public RefreshWindowModel()
        {
            Task RefreshAsync = Task.Run(async () => await Refresh.Refr());            

            RefreshAsync.ContinueWith(i =>
            {
                Rezult = Refresh.Message;
                CheckRun = !RefreshAsync.IsCompleted;
                CheckEnd = RefreshAsync.IsCompleted;
            });            
        }
    }
}
