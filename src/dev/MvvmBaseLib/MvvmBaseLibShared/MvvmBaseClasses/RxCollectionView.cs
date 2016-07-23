using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Reactive.Linq;


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
        private DataAndQueryCarrier<o> myDataSource = new DataAndQueryCarrier<o>();

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public RxCollectionView() : base()
        {
            SubscribeToEvents();
        }

        private void myDataSource_PropertiesSet(object sender, EventArgs e)
        {
            myResultingCollection = new ObservableCollection<o>(myDataSource.Query.ToList());

            var colObservable = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(myDataSource.Source, "CollectionChanged");

            var addedItemsObservable = from item in colObservable
                                       let NewItemSource = new DataAndQueryCarrier<o>
                                       {
                                           Query = this.Query,
                                           Source = new ObservableCollection<t>(item.EventArgs.NewItems.OfType<t>())
                                       }
                                       where item.EventArgs.Action == NotifyCollectionChangedAction.Add
                                       select new { Action = item.EventArgs.Action, NewItems = NewItemSource.Query };

            var myAddedItemsSubscription = addedItemsObservable.Subscribe((value) =>
            {
                if (value.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (var item in value.NewItems)
                    {
                        myResultingCollection.Add(item);
                    }
                }
            });

        }

        /// <summary>
        /// Query of the filter, defined as you would use LINQ.
        /// </summary>
        public IQueryable<o> Query
        {
            get
            {
                return myDataSource.Query;
            }
            set
            {
                myDataSource.Query = value;
            }
        }

        /// <summary>
        /// The source collection of type o.
        /// </summary>
        public INotifyCollectionChanged Source
        {
            get
            {
                return myDataSource.Source;
            }
            set
            {
                myDataSource.Source = value;
            }
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

        private bool EventsSubscribed = false;

        private void SubscribeToEvents()
        {
            if (EventsSubscribed)
                return;
            else
                EventsSubscribed = true;

            myDataSource.PropertiesSet += myDataSource_PropertiesSet;
        }

    }

    /// <summary>
    /// Infrastructure class for RxCollectionView for holding the RX-based query.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    public class DataAndQueryCarrier<t>
    {

        public delegate void PropertiesSetEventHandler(object sender, EventArgs e);
        public event PropertiesSetEventHandler PropertiesSet;

        private INotifyCollectionChanged mySource;
        private IQueryable<t> myQuery;

        protected virtual void OnPropertiesSet(EventArgs e)
        {
            if (PropertiesSet != null)
                PropertiesSet(this, e);
        }

        public IQueryable<t> Query
        {
            get
            {
                return myQuery;
            }
            set
            {
                if (!(object.Equals(myQuery, value)))
                {
                    myQuery = value;
                    if (value != null && Source != null)
                    {
                        OnPropertiesSet(EventArgs.Empty);
                    }
                }
            }
        }
        public INotifyCollectionChanged Source
        {
            get
            {
                return mySource;
            }
            set
            {
                if (!(object.Equals(mySource, value)))
                {
                    mySource = value;
                    if (value != null && Query != null)
                    {
                        OnPropertiesSet(EventArgs.Empty);
                    }
                }
            }
        }
    }
}