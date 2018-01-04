using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    public class ViewModelDataType
    {
        static List<ViewModelDataType> items;

        public static List<ViewModelDataType> Items
        {
            get
            {
                if (null == items)
                {
                    items = new List<ViewModelDataType>();

                    items.Add(new ViewModelDataType() { name = "text", size = 255 });
                    items.Add(new ViewModelDataType() { name = "int", size = 8 });
                    items.Add(new ViewModelDataType() { name = "bool" });
                    items.Add(new ViewModelDataType() { name = "datetime" });
                    items.Add(new ViewModelDataType() { name = "identity", size = 8 });
                }

                return items;
            }

            private set
            {
                items = value;
            }
        }

        public string name { get; set; }
        public int size { get; set; }
        public string mask { get; set; }
        public int minVal { get; set; }
        public int maxVal { get; set; }

        ViewModelDataType()
        {


        }
    }


}
