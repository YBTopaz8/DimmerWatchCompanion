using DimmerWatchCompanion.Utils;
using Parse;

namespace DimmerWatchCompanion;

public partial class MainPage : ContentPage
{
    
    public MainPage(MainPageVM ViewModel)
    {
        InitializeComponent();
        this.ViewModel = ViewModel;
        BindingContext = ViewModel;
    }

    public MainPageVM ViewModel { get; set; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ViewModel.LoginUser();
        ViewModel.SetupLiveQueries();
    }
    
    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var s = (View)sender;
        var song = (SongModelView)s.BindingContext;

        await ViewModel.PlaySong(song);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        ViewModel.SetupLiveQueries();
    }

    List<SongModelView>? filteredSongs = new();
    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBar = (SearchBar)sender;
        var txt = searchBar.Text;

        if (!string.IsNullOrEmpty(txt))
        {
            if (txt.Length >= 1)
            {

                // Setting the FilterString for SongsColView
                SongsColView.FilterString = $"Contains([Title], '{txt}') Or Contains([ArtistName], '{txt}')";
                filteredSongs?.Clear();

                // Apply the filter to the DisplayedSongs collection                
            }
        }
        else
        {
            SongsColView.FilterString = string.Empty;
        }
    }

}
