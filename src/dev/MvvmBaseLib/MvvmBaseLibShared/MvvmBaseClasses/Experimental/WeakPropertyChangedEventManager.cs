using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace MvvmBaseLibShared.MvvmBaseClasses.Experimental
{
    /// <summary>
    /// Allows to bind the PropertyChangedEvent from any ViewModel implementing INotifyPropertyChanged weak. 
    /// The type of the view does not matter.
    /// </summary>
    public class WeakPropertyChangedEventManager
    {
        internal delegate void OIPropertyChangedEventHandler<viewType>(
                                                   viewType view,
                                                   object sender,
                                                   PropertyChangedEventArgs e);

        WeakReference myTargetObject;
        Delegate myOpenInstanceDelegate;

        /// <summary>
        /// Returns a WeakReference to a WeakPropertyChangedEventManager.
        /// </summary>
        /// <param name="viewModel">The ViewModel whose PropertyChanged Event we want to bind weakly.</param>
        /// <param name="targetProc">The target event handler, we want to bind weakly.</param>
        /// <returns></returns>
        public static WeakReference<WeakPropertyChangedEventManager> AddHandlerWeak(
                                          INotifyPropertyChanged viewModel,
                                          PropertyChangedEventHandler targetProc)
        {
            return new WeakReference<WeakPropertyChangedEventManager>(
                new WeakPropertyChangedEventManager(viewModel,
                                     targetProc));
        }

        internal WeakPropertyChangedEventManager(INotifyPropertyChanged viewModel,
                                Delegate targetProc)
        {
            object view = targetProc.Target;
            myTargetObject = new WeakReference(view);

            var delegateType = typeof(OIPropertyChangedEventHandler<>).MakeGenericType(view.GetType());


            // !!!Does not work in PCLs.!!!
            //myOpenInstanceDelegate = Delegate.CreateDelegate(
            //    delegateType, null, targetProc.GetMethodInfo());

            //We need to use CreateDelegate from a MethodInfo Instance, which also creates an open instance
            //when we replace the target by null.
            myOpenInstanceDelegate = targetProc.GetMethodInfo().CreateDelegate(delegateType, null);
            (viewModel).PropertyChanged += PropertyChangedProc;
        }

        private void PropertyChangedProc(object sender, PropertyChangedEventArgs e)
        {
            //Re-Routing the Event.
            RaiseEvent(e);
        }

        private void RaiseEvent(PropertyChangedEventArgs e)
        {
            if (myOpenInstanceDelegate != null)
            {
                object tmpTarget;
                tmpTarget = myTargetObject.Target;
                if (tmpTarget != null)
                {
                    myOpenInstanceDelegate.DynamicInvoke(tmpTarget, this, e);
                }
            }
        }
    }
}
