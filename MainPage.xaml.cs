namespace Grosu_Olesea_Lab7
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"You clicked {count} time!";
            else
                CounterBtn.Text = $"You clicked {count} times!";

            if (count == 3)
            {
                DisplayAlert("Great job!", "Ai ajuns la 3 clicuri :D", "OK");
            }

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
