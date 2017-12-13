using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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

        private string FormName;

        public ucForm() : this("formulario")
        {

        }

        public ucForm(string formName)
        {
            FormName = formName;
            InitializeComponent();
            loadSchema();

        }
        public ucForm(string formName, ViewModelFormulario[] form)
        {
            FormName = formName;
            InitializeComponent();
            createControls(form);

        }

        public void loadSchema()
        {
            var client = new RestClient("http://127.0.0.1:8080/edsa-ksuit/http/");

            var _cookieJar = new CookieContainer();

            client.CookieContainer = _cookieJar;

            var request = new RestRequest("?op=SCHEMA&table=" + FormName, Method.GET);

            client.ExecuteAsync(request, response2 =>
            {
                Console.WriteLine(response2.Content);
                var data = JsonConvert.DeserializeObject<ViewModelFormulario[]>(response2.Content);
                createControls(data);

            });
        }

        private void createControls(ViewModelFormulario[] schema)
        {

            foreach (var formulario in schema)
            {
                this.Dispatcher.Invoke(new updateContextCallback(this.addField), formulario);

                foreach (var campo in formulario.fields)
                {
                    this.Dispatcher.Invoke(new updateContextCallback(this.addField), campo);
                }

                var firstSub = true;

                if (null != formulario.subs)
                    foreach (var sub in formulario.subs)
                    {
                        if (firstSub)
                        {

                            this.Dispatcher.Invoke(new updateContextCallback(this.addSub), sub);



                            //TODO ADICIONAR SUBFORMS NO TABCONTROL SE FIRSTSUB
                        }
                    }
            }
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            var d = -e.Delta;

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
            scrollButtons.Visibility = Visibility.Collapsed;
            scrollButtons2.Width = 180;
            scrollButtons2.HorizontalAlignment = HorizontalAlignment.Left;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            subtabs.Visibility = Visibility.Collapsed;
            scrollButtons.Visibility = Visibility.Visible;
            scrollButtons2.Width = double.NaN;
            scrollButtons2.HorizontalAlignment = HorizontalAlignment.Stretch;
        }

        private void addField(object o)
        {
            var t = new TextBox();
            t.Text = ((IViewModelNome)o).nome;
            mainStack.Children.Add(t);
        }
        private void addSub(object o)
        {
            var sub = (ViewModelFormulario)o;
            var form = new ucForm(sub.nome, new ViewModelFormulario[] { sub });

            var tabItem = new TabItem();
            tabItem.Header = sub.nome;
            tabItem.Content = form;
            subtabs.Items.Add(tabItem);

            var buttonItem = new Button();
            buttonItem.Click += Button_Click;
            buttonItem.Content = sub.nome;
            hiddenTabs.Children.Add(buttonItem);

        }

        public interface IViewModelNome
        {
            string nome { get; set; }
        }

        public class ViewModelFormulario : IViewModelNome
        {
            public int id;
            public string nome { get; set; }
            public string descricao;
            public string tabela;
            public string formulario;
            public string visualizacaoModo;
            public ViewModelCampo[] fields;
            public ViewModelFormulario[] subs;
        }

        public class ViewModelCampo : IViewModelNome
        {
            public int id;
            public string formulario;
            public string nome { get; set; }
            public int ordem;
            public string descricao;
            public string dadoTipo;
            public string obrigatorio;
            public string editavel;
            public string campo;
        }


    }
}
