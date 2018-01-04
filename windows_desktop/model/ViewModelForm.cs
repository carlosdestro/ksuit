using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    public class ViewModelForm : IViewModelName, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void SetPropertyField<T>(string propertyName, ref T field, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
        }


        private int _id;
        private string _name;
        private string _description;
        private string _tableName;
        private string _form;
        private string _viewMode;
        private string _position;
        private string _status;

        private Dictionary<string, object> _values;

        private ObservableCollection<ViewModelField> _fields;
        private ObservableCollection<ViewModelForm> _subs;
        private ObservableCollection<ViewModelOperation> _operations;

        
        public int id { get { return _id; } set { _id = value; } }
        public string name { get { return _name; } set { _name = value; } }
        public string description { get { return _description; } set { _description = value; } }
        public string tableName { get { return _tableName; } set { _tableName = value; } }
        public string form { get { return _form; } set { _form = value; } }
        public string viewMode { get { return _viewMode; } set { _viewMode = value; } }
        public string position { get { return _viewMode; } set { _viewMode = value; } }
        public string status { get { return _viewMode; } set { _viewMode = value; } }
        public ObservableCollection<ViewModelField> fields { get => _fields; set => SetPropertyField("fields", ref _fields, value); }
        public ObservableCollection<ViewModelForm> subs { get { return _subs; } set { _subs = value; } }
        public ObservableCollection<ViewModelOperation> operations { get { return _operations; } set { _operations = value; } }

        public Dictionary<string, object> values { get { return _values; } set { _values = value; } }

        public ViewModelForm parent { get; set; }

        public ViewModelForm()
        {
            _fields = new ObservableCollection<ViewModelField>();
            _subs = new ObservableCollection<ViewModelForm>();
            _operations = new ObservableCollection<ViewModelOperation>();
        }

        public ObservableCollection<T> Convert<T>(IEnumerable<T> original)
        {
            return new ObservableCollection<T>(original);
        }

        public object Clone()
        {
            var result = new ViewModelForm();

            result._id = _id;
            result._name = _name;
            result._description = _description;
            result._tableName = _tableName;
            result._form = _form;
            result._viewMode = _viewMode;
            result._position = _position;
            result._status = _status;
            result._fields = Convert(_fields.Select(x => (ViewModelField)x.Clone()));
            result._subs= _subs;
            result._operations = _operations;

            return result;
        }

        public Dictionary<string, object> Serialize()
        {
            var result = new Dictionary<string, object>();

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];

                result.Add(field.name, field.Value);
            }

            //if (null != form.subs)
            //    for (var i = 0; i < form.subs.Length; i++)
            //    {
            //        var sub = form.subs[i];
            //        result.Add(sub.nome, ((ucForm)((TabItem)subtabs.Items[i]).Content).Serialize());
            //    }

            return result;
        }

        public void Bind(object o)
        {
            var record = (Dictionary<string, object>)o ;

            if(null == record["values"])
            {
                for (var i = 0; i < fields.Count; i++)
                {
                    var field = fields[i];

                    field.Value = null;
                }

                return;
            }

            var t = JsonConvert.DeserializeObject<Dictionary<string, object>>(record["values"].ToString());

            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];


                field.Value = t [fields[i].name]?.ToString();
            }

        }
    }
}
