using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    public class BindingErrorTraceListener : TraceListener
    {
        public override void Write(string s) { }

        public override void WriteLine(string message)
        {
            //throw new Exception(message);
        }
    }
}
