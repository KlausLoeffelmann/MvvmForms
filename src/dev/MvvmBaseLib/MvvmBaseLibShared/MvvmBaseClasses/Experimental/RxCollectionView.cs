using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Reactive.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{

    /// <summary>
    /// Represents a ReactiveExtension based collection wrapper class, where when you add an element t to the Source, 
    /// it gets filtered through a query and a correlating collection of o is been updated according to the filter query.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <typeparam name="o"></typeparam>
    public class RxCollectionView<t, o>
    {

        private ObservableCollection<o> myResultingCollection;

        private bool EventsSubscribed = false;
        private MutatingSource<t> mySource;
        private IEnumerable<o> myQuery;
        private ObservableCollection<t> myObservableSource;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public RxCollectionView(ObservableCollection<t> observableSource) : base()
        {
            mySource = new MutatingSource<t>(observableSource);
            myObservableSource = observableSource;
        }

        private void SubscribeToEvents()
        {
            if (EventsSubscribed)
                return;
            else
            {
                EventsSubscribed = true;

                myResultingCollection = new ObservableCollection<o>(Query);
                //if (Debugger.IsAttached)
                //    Debugger.Break();

                var colObservable = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(myObservableSource, "CollectionChanged");


                var addedItemsObsTest = from item in colObservable
                                        where item.EventArgs.Action == NotifyCollectionChangedAction.Add
                                        select item;

                var myAddedItemsSubTest = addedItemsObsTest.Subscribe((value) =>
                {

                    if (value.EventArgs.Action == NotifyCollectionChangedAction.Add)
                    {
                        //if (Debugger.IsAttached)
                        //    Debugger.Break();

                        mySource.Source = new MutatingSource<t>(value.EventArgs.NewItems.OfType<t>());

                        foreach (var item in Query.ToList())
                        {
                            myResultingCollection.Add(item);
                        }
                    }
                });

            }

        }

        /// <summary>
        /// Query of the filter, defined as you would use LINQ.
        /// </summary>
        public IEnumerable<o> Query
        {
            get
            {
                return myQuery;
            }
            set
            {
                myQuery = value;
                if (myQuery!=null)
                {
                    SubscribeToEvents();
                }
            }
        }

        /// <summary>
        /// Source Collection
        /// </summary>
        public MutatingSource<t> Source
        {
            get { return mySource; }
            set { mySource = value; }
        }

        /// <summary>
        /// The filtered outcome of the collection as ObservableCollection.
        /// </summary>
        public ObservableCollection<o> ResultingCollection
        {
            get
            {
                return myResultingCollection;
            }
        }
    }

#pragma warning disable 1591
    public class MutatingSource<t> : IEnumerable<t>
    {
        public MutatingSource(IEnumerable<t> originalSource)
        {
            this.Source = originalSource;
        }

        public IEnumerable<t> Source { get; set; }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IEnumerator<t> GetEnumerator()
        {
            return Source.GetEnumerator();
        }
    }
#pragma warning restore 1591

}
