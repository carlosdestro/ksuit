using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    public class ViewModelOperation : IViewModelName
    {
        public int id { get; set; }
        public string name { get; set; }
        public string objectType;
        public string objectName;
        public string operationType;
        public int position;
        public string status;
        public string tsql;
    }
}
