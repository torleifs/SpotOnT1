using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using SpotOnT1.Login;

namespace SpotOnT1.ViewModels
{
    class PlaylistOverviewViewModel : INotifyPropertyChanged
    {
        private ISpotifyClient _spotifyClient;
        private ILoginService _loginService;

        public PlaylistOverviewViewModel(ISpotifyClient spotifyClient, ILoginService loginService)
        {
            _spotifyClient = spotifyClient;
            _loginService = loginService;
            Task.Run(async () => await Populate());
        }
        public string Title { get => "Playlists"; }

        private ObservableCollection<PlayList> _playlists;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<PlayList> PlayLists
        {
            get => _playlists;
            set
            {
                _playlists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayLists"));
            }

        }

       // public Command ItemClickedCommand => new Command(async () => await)
        public async Task Populate()
        {
            var rawPlayLists = await _spotifyClient.GetPlayLists("Bearer " + _loginService.AuthToken);
            PlayLists = new ObservableCollection<PlayList>(rawPlayLists.items);
        }
    }
}