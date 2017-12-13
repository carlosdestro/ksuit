using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
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

namespace lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Field> Fields { get; set; }
        private CookieContainer cookieContainer = new CookieContainer();

        public MainWindow()
        {
            InitializeComponent();

           



        }

       

        

        public class Field
        {
            public string Name { get; set; }
            public int Length { get; set; }
            public bool Required { get; set; }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            var formName = ((Button)sender).Content.ToString();

            var form = new ucForm(formName);

            var tabItem = new TabItem();
            tabItem.Header = formName;
            tabItem.Content = form;

            tabControl.Items.Add(tabItem);
        }
    }
}