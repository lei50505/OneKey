using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OneKey.window;

namespace OneKey
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

       

     

        private void MenuChgPsw_Click(object sender, RoutedEventArgs e)
        {
            WindowChgPsw wcp = new WindowChgPsw();
            wcp.windowMain = this;
            wcp.Owner = this;
            this.IsEnabled = false;
            wcp.Show();
        }

   

        private void MenueExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}
