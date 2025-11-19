namespace Tienda
{
    public partial class MainPage : ContentPage
    {
        private IDispatcherTimer timer;

        public MainPage()
        {
            InitializeComponent();

            timer = Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(1);

            timer.Tick += (s, e) =>
            {
                FechaHoraLabel.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss");
            };
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            timer.Start();
        }

    

    }

}
