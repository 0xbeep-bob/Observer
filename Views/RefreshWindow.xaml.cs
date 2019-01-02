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
using System.Windows.Shapes;

namespace Observer.Views
{
    public partial class RefreshWindow : Window
    {
        public RefreshWindow()
        {
            InitializeComponent();
        }

        // закрытие окна по клику
        private void escButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // закрытие окна
        }

        // закрытие окна по кнопкам
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Escape || e.Key == Key.Enter) && escButton.IsEnabled)
            {
                Close();
            }
        }
    }
}
