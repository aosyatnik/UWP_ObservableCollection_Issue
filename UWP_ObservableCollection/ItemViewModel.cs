namespace UWP_ObservableCollection
{
    public class ItemViewModel : BaseViewModel
    {
        private static int Counter;
        public string Text { get; private set; }

        public ItemViewModel()
        {
            Counter++;
            Text = $"{Counter}";
        }
    }
}
