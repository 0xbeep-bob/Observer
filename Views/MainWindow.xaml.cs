using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using xNet;
using System.Data.SQLite;
using System.Data.Entity;
using Observer.ViewModels;
using System.Text.RegularExpressions;

namespace Observer.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Regex BpInputRegex = new Regex(@"^[a-zA-Z0-9-.]$");
        Regex SusInputRegex = new Regex(@"^[a-zA-Z0-9-]$");
        
        // вылидация ввода Broker's Fee
        private void TraderTax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "." && ((sender as TextBox).Text.Length < 1 || (sender as TextBox).Text.Length >= 3))
            {
                e.Handled = true;
            }
        }        

        // валидация ввода чертежа
        private void BpFind_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Match match = BpInputRegex.Match(e.Text);
            if (!match.Success)
            {
                e.Handled = true;
            }
        }

        // валидация ввода системы
        private void SysFind_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Match match = SusInputRegex.Match(e.Text);
            if (!match.Success)
            {
                e.Handled = true;
            }
        }

        // валидация ввода числа ранов
        private void Runs_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = "0123456789".IndexOf(e.Text) < -1;
        }

        // запрет пробела
        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        // свич датагрида 
        private void RawResource_Checked(object sender, EventArgs e)
        {
            DataGrid.Visibility = (Visibility)1;
            DataGridRaw.Visibility = 0;
            RawProfit.Visibility = 0;
        }

        // свич датагрида 
        private void RawResource_Unchecked(object sender, EventArgs e)
        {
            DataGrid.Visibility = 0;
            DataGridRaw.Visibility = (Visibility)1;
            RawProfit.Visibility = (Visibility)1;
        }

        // свич комбобокса с бонусами
        private void GridBonuses_Checked(object sender, EventArgs e)
        {
            GridBonuses.Visibility = (Visibility)1;
            GridBonusesComponent.Visibility = 0;
        }

        // свич комбобокса с бонусами
        private void GridBonuses_UnChecked(object sender, EventArgs e)
        {
            GridBonuses.Visibility = 0;
            GridBonusesComponent.Visibility = (Visibility)1;
        }

        // валидация ввода только цифр и 1 точки для текстбоксов налогов
        private void DigitAndOneDot_TextChanged(object sender, EventArgs e)
        {
            string tmp = (sender as TextBox).Text.Trim();
            string outS = string.Empty;
            bool dot = true;

            foreach (char ch in tmp)
                if (char.IsDigit(ch) || (ch == '.' && dot))
                {
                    outS += ch;
                    if (ch == '.')
                        dot = false;
                }

            (sender as TextBox).Text = outS;
            (sender as TextBox).SelectionStart = outS.Length;
        }

        private void RefreshBase_Click(object sender, RoutedEventArgs e)
        {
            RefreshWindow rr = new RefreshWindow();
            rr.ShowDialog();
        }
    }
}