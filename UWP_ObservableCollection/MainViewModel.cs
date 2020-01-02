using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UWP_ObservableCollection
{
    public class MainViewModel : BaseViewModel
    {
        public IList<ItemViewModel> ItemsAsList { get; private set; }
        public ObservableCollection<ItemViewModel> ItemsAsObservableCollection { get; private set; }
        public IList<ItemViewModel> ItemsRecreatedList { get; private set; }

        public MainViewModel()
        {
            ItemsAsList = new List<ItemViewModel>();
            ItemsAsObservableCollection = new ObservableCollection<ItemViewModel>();
            ItemsRecreatedList = new List<ItemViewModel>();
        }

        public void AddNewItem()
        {
            var newItem = new ItemViewModel();

            // First try: add to list and raise property change - doesn't work.
            ItemsAsList.Add(newItem);
            RaisePropertyChanged(nameof(ItemsAsList));

            // Second try: with ObservableCollection - doesn't work?
            ItemsAsObservableCollection.Add(newItem);

            // Third try: recreate the whole collection - works
            ItemsRecreatedList.Add(newItem);
            ItemsRecreatedList = new List<ItemViewModel>(ItemsRecreatedList);
            RaisePropertyChanged(nameof(ItemsRecreatedList));
        }
    }
}
