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
    /// WindowLogin.xaml 的交互逻辑
    /// </summary>
    public partial class WindowLogin : Window
    {
        public MainWindow mainWindow=null;
        public bool callClose=false;
        public WindowLogin()
        {
            InitializeComponent();
        }
        

        private void Window_Closed(object sender, EventArgs e)
        {
            if (callClose)
            {
                return;
            }

            mainWindow.windowLogin = null;
            mainWindow.Close();
               
            
            
            
        }

        private void ButtonDo_Click(object sender, RoutedEventArgs e)
        {
            string psw = PswBox.Password.ToString();
            if (string.IsNullOrWhiteSpace(psw))
            {
                MessageBox.Show("密码不能为空");
                return;
            }
            string md5Psw = UMD5.strToSaltBase64Str(psw);
            if (UFile.validPsw(md5Psw))
            {
                mainWindow.windowLogin = null;
                this.callClose = true;
                mainWindow.IsEnabled = true;
                mainWindow.psw = psw;
                this.Close();
                mainWindow.startLoadTitlesThread();
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }
       
    }
}
