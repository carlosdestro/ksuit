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

namespace lab
{
    /// <summary>
    /// Interaction logic for ucForm.xaml
    /// </summary>
    public partial class ucForm : UserControl
    {
        public delegate void updateContextCallback(object o);

        public string FormName;

        public ucForm() : this("formulario")
        {

        }

        public ucForm(string formName)
        {
            FormName = formName;

            InitializeComponent();

            loadSchema();
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

                foreach (var formulario in data)
                {
                    this.Dispatcher.Invoke(new updateContextCallback(this.updateContext), formulario);

                    foreach (var campo in formulario.fields)
                    {
                        this.Dispatcher.Invoke(new updateContextCallback(this.updateContext), campo);
                    }

                    var firstSub = true;

                    foreach(var sub in formulario.subs)
                    {
                        if(firstSub)
                        {
                            var tabControl = new TabControl();

                           //TODO ADICIONAR SUBFORMS NO TABCONTROL SE FIRSTSUB
                        }
                    }
                }


            });
        }

        private void updateContext(object o)
        {
            var t = new TextBox();
            t.Text = ((IViewModelNome)o).nome;
            mainStack.Children.Add(t);
        }

        interface IViewModelNome
        {
            string nome { get; set; }
        }

        class ViewModelFormulario : IViewModelNome
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

        class ViewModelCampo : IViewModelNome
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
