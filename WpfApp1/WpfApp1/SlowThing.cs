using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfApp1
{
    class SlowThing
    {
        public string Data
        {
            get
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                return "Hi!";
            }
        }
    }
}
