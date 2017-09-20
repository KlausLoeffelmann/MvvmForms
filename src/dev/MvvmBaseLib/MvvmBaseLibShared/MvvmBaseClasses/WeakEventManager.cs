using System;
using System.Linq;
using System.Reflection;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
    /// <summary>
    /// Provides a base class for the event manager that is used in the weak event pattern. The manager adds and removes listeners for events (or callbacks) that also use the pattern.
    /// </summary>
    public class WeakEventManager
    {
        /// <summary>
        /// Adds the specified event handler to the specified event.
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="sender">The source object that raises the specified event.</param>
        /// <param name="eventHandler">The delegate that handles the event.</param>
        /// <param name="eventName">The name of the event to subscribe to.</param>
        public static void AddHandler<TEventArgs>(object sender, EventHandler<TEventArgs> eventHandler, string eventName)
        {
            var listener = eventHandler.Target;
            var openInstanceDelegate = eventHandler.GetMethodInfo().CreateDelegate(eventHandler.GetType(), null);
            var eventInfo = sender.GetType().GetRuntimeEvent(eventName);

            var proxy = new EventBinderProxy(sender, listener, openInstanceDelegate, eventInfo, eventHandler);
        }

        /// <summary>
        /// Removes the specified event handler from the specified event.
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="sender">The source object that raises the specified event.</param>
        /// <param name="eventHandler">The delegate to remove</param>
        /// <param name="eventName">The name of the event to remove the handler from.</param>
        public static void RemoveHandler<TEventArgs>(object sender, EventHandler<TEventArgs> eventHandler, string eventName)
        {
            var eventInfo = sender.GetType().GetRuntimeEvent(eventName);
            var eventField = eventInfo.DeclaringType.GetRuntimeFields().Where(t => t.Name == eventName).Single();
            var eventIni = eventField.GetValue(sender) as Delegate;
            var delegates = eventIni.GetInvocationList();

            foreach (var del in delegates)
            {
                if (del.Target is EventBinderProxy target)
                {
                    //compare listener and target handler!
                    if (eventHandler.Target == target.Listener.Target && target.OpenInstanceDelegate.GetMethodInfo().Name == eventHandler.GetMethodInfo().Name)
                    {
                        eventInfo.RemoveEventHandler(sender, del);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Internal proxy class that is used in the manager for the re-routing of the event
        /// </summary>
        private class EventBinderProxy
        {
            /// <summary>
            /// Weak reference to the listener
            /// </summary>
            public WeakReference Listener { get; private set; }

            private string _eventName;

            /// <summary>
            /// The sender of the event (strong binding)
            /// </summary>
            public object Sender { get; private set; }

            /// <summary>
            /// An open delegate to the origin handler without any instancebinding
            /// </summary>
            public Delegate OpenInstanceDelegate { get; private set; }

            public EventBinderProxy(object sender, object listener, Delegate openInstanceDelegate, EventInfo eventInfo, Delegate listenerProc)
            {
                EventHandler proxyHandler = EventHandlerProxy;
                Sender = sender;
                Listener = new WeakReference(listener);
                _eventName = eventInfo.Name;
                OpenInstanceDelegate = openInstanceDelegate;

                var internalHandler = Cast(proxyHandler, eventInfo.EventHandlerType);

                eventInfo.AddEventHandler(sender, internalHandler);
            }

            /// <summary>
            /// Handles the source event and delegates it to the origin object
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public void EventHandlerProxy(object sender, EventArgs e)
            {
                //Re-Routing the Event.
                if (OpenInstanceDelegate != null && Listener.IsAlive)
                {
                    OpenInstanceDelegate.GetMethodInfo().Invoke(Listener.Target, new object[] { sender, e });
                }
                else
                {
                    //remove handler
                    EventHandler proxyHandler = EventHandlerProxy;
                    var eventInfo = sender.GetType().GetRuntimeEvent(_eventName);
                    var internalHandler = Cast(proxyHandler, eventInfo.EventHandlerType);

                    eventInfo.RemoveEventHandler(sender, internalHandler);
                    this.Sender = null;
                }
            }

            /// <summary>
            /// Cast the specific delegate to the typed version
            /// </summary>
            /// <param name="source">Abstract Delegate</param>
            /// <param name="type">Target Type</param>
            /// <returns>Typed Delegate</returns>
            public static Delegate Cast(Delegate source, Type type)
            {
                if (source == null) return null;

                var delegates = source.GetInvocationList();

                if (delegates.Length == 1)
                {
                    var del = delegates.Single();
                    return del.GetMethodInfo().CreateDelegate(type, del.Target);
                }
                else
                {
                    var delegatesDest = new Delegate[delegates.Length];

                    for (int nDelegate = 0; nDelegate < delegates.Length; nDelegate++)
                    {
                        delegatesDest[nDelegate] = delegates[nDelegate].GetMethodInfo().CreateDelegate(type, delegates[nDelegate].Target);
                    }

                    return Delegate.Combine(delegatesDest);
                }
            }
        }
    }

}
