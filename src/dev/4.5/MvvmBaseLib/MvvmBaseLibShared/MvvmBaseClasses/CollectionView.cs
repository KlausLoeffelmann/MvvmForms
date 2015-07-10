using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Specialized;
using System.Reactive.Linq;

namespace ActiveDevelop.MvvmBaseLib.Mvvm
{
        public class CollectionView<t>
		{

			private ObservableCollection<t> myResultingCollection;
			private DataAndQueryCarrier<t> myDataSource = new DataAndQueryCarrier<t>();

			public CollectionView() : base()
			{
				SubscribeToEvents();
			}

			private void myDataSource_PropertiesSet(object sender, EventArgs e)
			{
				myResultingCollection = new ObservableCollection<t>(myDataSource.Query.ToList());

				var colObservable = Observable.FromEventPattern<NotifyCollectionChangedEventArgs>(myDataSource.Source, "CollectionChanged");

				var addedItemsObservable = from item in colObservable
				                           let NewItemSource = new DataAndQueryCarrier<t> {Query = this.Query, Source = new ObservableCollection<t>(item.EventArgs.NewItems.OfType<t>())}
				                           where item.EventArgs.Action == NotifyCollectionChangedAction.Add
				                           select new {Action = item.EventArgs.Action, NewItems = NewItemSource.Query};

				var myAddedItemsSubscription = addedItemsObservable.Subscribe((value) => {
					if (value.Action == NotifyCollectionChangedAction.Add)
					{
						foreach (var item in value.NewItems)
						{
							myResultingCollection.Add(item);
						}
					}
				});

			}

			public IQueryable<t> Query
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

			public ObservableCollection<t> ResultingCollection
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