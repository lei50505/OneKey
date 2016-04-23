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
using OneKey.src.util;

namespace OneKey.window
{
    /// <summary>
    /// WindowAddPsw.xaml 的交互逻辑
    /// </summary>
    public partial class WindowAddPsw : Window
    {
        public MainWindow mainWindow=null;
        public WindowAddPsw()
        {
            InitializeComponent();
        }
        public bool callClose = false;
        private void Window_Closed(object sender, EventArgs e)
        {
            if (callClose)
            {
                return;
            }
            mainWindow.windowAddPsw =null;
            mainWindow.Close();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string psw1=PswBoxFirst.Password.ToString();
            string psw2 = PswBoxSecond.Password.ToString();
            if (string.IsNullOrWhiteSpace(psw1))
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            if (!psw1.Equals(psw2))
            {
                MessageBox.Show("两次密码不相同");
                return;
            }
            string psw = UMD5.strToSaltBase64Str(psw1);
            UFile.createPsw(psw);
            mainWindow.windowAddPsw = null;
            this.callClose = true;
            this.Close();
            mainWindow.IsEnabled = true;
            mainWindow.psw = psw1;
        }
    }
}
