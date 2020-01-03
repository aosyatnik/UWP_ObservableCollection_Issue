# UWP_ObservableCollection_Issue
This repo shown UWP with ObservableCollection problem. Maybe it's not a problem and I don't understand something. So I'm asking the community to help me to figure out if it's my mistake.

# Steps to reproduce
1. Build and run app.
2. See there are 3 `MyItemsControl` that are using 3 different data sources - `ItemsAsList`, `ItemsAsObservableCollection`and `ItemsRecreatedList`.
Check `MainViewModel` and find, that there are 3 sources:
* `IList<ItemViewModel> ItemsAsList`
* `ObservableCollection<ItemViewModel> ItemsAsObservableCollection`
* `IList<ItemViewModel> ItemsRecreatedList`
3. Click on "Add new item". You should see, that 2nd and 3rd collections are updated. Check in `MainViewModel` method called `AddNewItem.
It should add the item to each collection. 
### **First question: why the item is added to the first collection, but UI is not updated even if RaisePropertyChanged is called?**
----

4. Stop app.
5. Go to `MyItemsControl.xaml.cs` find commented code, uncomment it and comment previous code. This changes `IList<ItemViewModel>` to `IList<BaseViewModel>`.

Your file should look like this:
``` csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP_ObservableCollection
{
    public sealed partial class MyItemsControl : UserControl
    {
        // This works fine.
        /*public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(
                "ItemsSource",
                typeof(IList<ItemViewModel>),
                typeof(MyItemsControl),
                new PropertyMetadata(null, ItemsSourcePropertyChanged)
            );

        public IList<ItemViewModel> ItemsSource
        {
            get { return (IList<ItemViewModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }*/

        // Uncomment this code to see the issue.

        public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register(
               "ItemsSource",
               typeof(IList<BaseViewModel>),
               typeof(MyItemsControl),
               new PropertyMetadata(null, ItemsSourcePropertyChanged)
           );

        public IList<BaseViewModel> ItemsSource
        {
            get
            {
                var values = GetValue(ItemsSourceProperty) as IEnumerable<BaseViewModel>;
                if (values is null)
                {
                    return null;
                }
                return values.ToList();
            }
            set { SetValue(ItemsSourceProperty, value); }
        }


        private static void ItemsSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Items changed");
        }

        public MyItemsControl()
        {
            this.InitializeComponent();
        }
    }
}

```
6. Now rebuild app and run it once more. Try to click again on "Add new item" and notice, that `ObservableCollection` is not updated. 
### **Second question: why ObservableCollection doesn't trigger getter anymore?**

# Solution
Here I'll write the solution. I hope, that community will help me to find it out.

Answer:
1. List doesn't implement `INotifyCollectionChanged` that's why it's not updated.
2. `values.ToList()` creates new collection, that is disconnected from `MainViewModel`. To fix it you should write getter like this:

```csharp
public static readonly DependencyProperty ItemsSourceProperty =
           DependencyProperty.Register(
               "ItemsSource",
               typeof(IEnumerable<BaseViewModel>),
               typeof(MyItemsControl),
               new PropertyMetadata(null, ItemsSourcePropertyChanged)
           );

        public IEnumerable<BaseViewModel> ItemsSource
        {
            get { return (IEnumerable<BaseViewModel>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
```
