using MaterialDesignThemes.Wpf;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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

namespace windows_desktop
{

    /// <summary>
    /// Interaction logic for ucForm.xaml
    /// </summary>
    public partial class ucForm : UserControl
    {
        public delegate void updateContextCallback(object o);

        public ViewModelForm Form
        {
            get
            {
                return (ViewModelForm)DataContext;
            }
            set
            {
                DataContext = value;
            }
        }

        RestClient client;

        public ucForm()
        {
            InitializeComponent();

            client = new RestClient("http://127.0.0.1:8080/edsa-ksuit/http/");

            var _cookieJar = new CookieContainer();

            client.CookieContainer = _cookieJar;
        }

        private Dictionary<string, object> Serialize()
        {
            var result = new Dictionary<string, object>();

            for (var i = 0; i < Form.fields.Count; i++)
            {
                var field = Form.fields[i];

                result.Add(field.name, ((ViewModelField)mainStack.Items[i]).Value);
            }

            return result;
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var d = -e.Delta;

            var scrollButtons = (ScrollViewer)sender;

            if (d < 0)
            {
                if (scrollButtons.HorizontalOffset + d > 0)
                {
                    scrollButtons.ScrollToHorizontalOffset(scrollButtons.HorizontalOffset + d);
                }
                else
                {
                    scrollButtons.ScrollToLeftEnd();
                }
            }
            else
            {
                if (scrollButtons.ExtentWidth > scrollButtons.HorizontalOffset + d)
                {
                    scrollButtons.ScrollToHorizontalOffset(scrollButtons.HorizontalOffset + d);
                }
                else
                {
                    scrollButtons.ScrollToRightEnd();
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            subtabs.Visibility = Visibility.Visible;
            gridScrollButtons.Visibility = Visibility.Collapsed;
            mainStack.Width = 880;
            mainStack.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            subtabs.Visibility = Visibility.Collapsed;
            gridScrollButtons.Visibility = Visibility.Visible;
            mainStack.Width = double.NaN;
            mainStack.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
 
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            var operation = ((Button)sender).Tag.ToString();

            if (null != Form.parent)
                Form.fields.FirstOrDefault( x=> x.name == Form.parent.name).Value = Form.parent.fields[1].Value;

            if (Form.fields[0].Value == null)
                Form.fields[0].Value = "0";

            var d = Form.Serialize();

            var del = new updateContextCallback(Form.Bind);

            HttpHelper.ExecuteAsync(operation, Form.name, response2 =>
            {
                var record = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(response2.Content).First();

                this.Dispatcher.Invoke(del, record);

            }, d);
        }

        public ViewModelForm GetForm()
        {
            return (ViewModelForm)Form.Clone();
        }
    }

    public class FormTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var fe = (FrameworkElement)container;
            var field = (ViewModelField)item;
            var prop = field?.dataType;

            try
            {
                if (null != prop)
                {
                    var res = fe.FindResource(prop + (field.required ? "_required" : string.Empty));

                    if (null != res)
                        return (DataTemplate)res;
                }

            }
            catch(Exception e)
            {

            }
            var res2 = fe.FindResource("text") as DataTemplate;


            if (null != res2)
                return res2;

            return base.SelectTemplate(item, container);
        }
    }
}
