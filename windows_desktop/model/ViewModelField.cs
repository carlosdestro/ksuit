using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    public class ViewModelField : IViewModelName, INotifyPropertyChanged, ICloneable
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

        public object Clone()
        {
            var result = new ViewModelField();

            result._id = _id;
            result._form = _form;
            result._name = _name;
            result._position = _position;
            result._description = _description;
            result._dataType = _dataType;
            result._required = _required;
            result._persistent = _persistent;
            result._field = _field;
            result._visible = _visible;
            result._value = _value;
            result._lookupTable = _lookupTable;
            result._lookupTableIdColumns = _lookupTableIdColumns;
            result._lookupTableLabelColumns = _lookupTableLabelColumns;
            

            return result;
        }

        private int _id;
        private string _form;
        private string _name;
        private string _description;
        private string _field;
        private string _dataType;
        private bool _required;
        private bool _persistent;
        private bool _visible;
        private int _position;
        private bool _status;
        private bool _calculated;

        private string _lookupTable;
        private string _lookupTableIdColumns;
        private string _lookupTableLabelColumns;


        private string _value;
        private ViewModelDataType _DataType;

        public int id { get => _id; set => SetPropertyField("id", ref _id, value); }
        public string form { get => _form; set => SetPropertyField("form", ref _form, value); }
        public string name { get => _name; set => SetPropertyField("name", ref _name, value); }
        public string description { get => _description; set => SetPropertyField("ddescription", ref _description, value); }
        public string field { get => _field; set => SetPropertyField("field", ref _field, value); }
        public string dataType { get => _dataType; set => SetPropertyField("dataType", ref _dataType, value); }

        [JsonConverter(typeof(BoolConverter))]
        public bool required { get => _required; set => SetPropertyField("required", ref _required, value); }

        [JsonConverter(typeof(BoolConverter))]
        public bool persistent { get => _persistent; set => SetPropertyField("persistent", ref _persistent, value); }

        [JsonConverter(typeof(BoolConverter))]
        public bool visible { get => _visible; set => SetPropertyField("visible", ref _visible, value); }
        public int position { get => _position; set => SetPropertyField("position", ref _position, value); }

        public string lookupTable { get => _lookupTable; set => SetPropertyField("lookupTable", ref _lookupTable, value); }
        public string lookupTableIdColumns { get => _lookupTableIdColumns; set => SetPropertyField("lookupTableIdColumns", ref _lookupTableIdColumns, value); }
        public string lookupTableLabelColumns { get => _lookupTableLabelColumns; set => SetPropertyField("lookupTableLabelColumns", ref _lookupTableLabelColumns, value); }



        [JsonConverter(typeof(BoolConverter))]
        public bool status { get => _status; set => SetPropertyField("status", ref _status, value); }
        public string Value { get => _value; set => SetPropertyField("Value", ref _value, value); }

        [JsonConverter(typeof(BoolConverter))]
        public bool calculated { get => _calculated; set => SetPropertyField("calculated", ref _calculated, value); }

        public ViewModelDataType DataType
        {
            get
            {
                if (null == _DataType)
                    _DataType = ViewModelDataType.Items.First(x => x.name == dataType);

                return _DataType;
            }

            set
            {
                _DataType = value;
            }
        }
    }
}
