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
using System.Windows.Shapes;

namespace OneKey.window
{
    /// <summary>
    /// WindowAbout.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAbout : Window
    {
        public MainWindow mainWindow;
        public bool callClose = false;

        public WindowAbout()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (callClose)
            {
                return;
            }
            mainWindow.IsEnabled = true;
            mainWindow.windowAbout = null;
            mainWindow.TextBoxKey.Focus();
        }
    }
}
