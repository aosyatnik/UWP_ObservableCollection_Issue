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
        public static readonly DependencyProperty ItemsSourceProperty =
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
        }

        // Uncomment this code to see the issue.
        /*
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
        */

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
