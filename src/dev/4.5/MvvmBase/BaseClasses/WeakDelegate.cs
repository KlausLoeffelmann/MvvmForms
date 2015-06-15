using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDev.MvvmBaseLib.BaseClasses
{
    class WeakDelegate : WeakReference 
    {
        private Delegate myWeakDelegate;

        public WeakDelegate(Delegate del):base(del)
        {

        }
    }
}
