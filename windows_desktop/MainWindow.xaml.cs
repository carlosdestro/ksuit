using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModelSearchResult searchResults = new ViewModelSearchResult();

        public delegate void updateContextCallback(object o);

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = searchResults;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ucHome_Navigate(object sender, RoutedEventArgs e)
        {
            var formName = ((Button)sender).Content.ToString();

            loadSchema(formName);
        }

        public void loadSchema(string formName)
        {
            HttpHelper.ExecuteAsync("SCHEMA", formName, response2 =>
            {
                this.Dispatcher.Invoke(new updateContextCallback(this.addForm), response2.Content);
            });
        }

        private void addForm(object o)
        {
            var form = JsonConvert.DeserializeObject<ViewModelForm[]>(o.ToString()).First();

            foreach(var s in form.subs)
                s.parent = form;

            var ucform = new ucForm { DataContext = form };

            var ti = new TabItem();

            ti.Header = ucform.Form.name;

            ti.Content = ucform;

            mainTabControl.Items.Add(ti);
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            var form = ((ucForm)((TabItem)mainTabControl.SelectedItem).Content).Form;

            var operation = form.operations.FirstOrDefault(x => x.name.Equals("PESQUISAR")).id;

            if (string.IsNullOrWhiteSpace(SearchBox.Text))
                return;

            Search(operation.ToString(), form);

        }

        private void Search(string operation, ViewModelForm form)
        {
            form = (ViewModelForm)form.Clone();

            form.fields[0].Value = SearchBox.Text;

            var d = form.Serialize();

            HttpHelper.ExecuteAsync(operation, form.name, response2 =>
            {
                var records = JsonConvert.DeserializeObject<ViewModelForm[]>(response2.Content);

                //Bind(records);

                this.Dispatcher.Invoke(new updateContextCallback(Bind), records);

            }, d);

        }

        public void Bind(object o)
        {
            var forms = (ViewModelForm)o;

            //foreach (var f in forms)
            searchResults.items.Add(forms);
        }


        private void listboxSearchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var form = (ucForm)((TabItem)this.mainTabControl.Items[1]).Content;

            var formValues = (ViewModelForm)((ListBox)sender).SelectedItem;


            this.Dispatcher.Invoke(new updateContextCallback(form.Form.Bind), formValues.values);

        }
    }
}
