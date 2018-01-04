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

namespace windows_desktop
{
    /// <summary>
    /// Interaction logic for ucHome.xaml
    /// </summary>
    public partial class ucHome : UserControl
    {
        public delegate void NavigateHandler(object sender, RoutedEventArgs e);

        public event NavigateHandler Navigate;

        public ucHome()
        {
            InitializeComponent();
        }

        private void asd_Click(object sender, RoutedEventArgs e)
        {   
            if(((Button)sender).Content.ToString() != "form")
                ((Button)sender).Content = "t" + textbox1.Text;

            if (null != Navigate)
                Navigate(sender, new RoutedEventArgs());
        }
    }
}
