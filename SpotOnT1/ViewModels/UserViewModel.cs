using System;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SpotOnT1.Login;
using System.ComponentModel;
namespace SpotOnT1.ViewModels
{
    public class UserViewModel : INotifyPropertyChanged
    {
        //  public string Name { get; set; }
        private string _name;
        public string Name {
            get => _name;
            set
            {
                if (value != Name) {
                    _name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                
                }
            }}

        private ObservableCollection<PlayList> _playlists;
        public ObservableCollection<PlayList> PlayLists {
            get => _playlists;
            set
            {
                _playlists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PlayLists"));
            }
        }
        private ISpotifyClient _spotifyClient;
        private ILoginService _loginService;

        public event PropertyChangedEventHandler PropertyChanged;

        public UserViewModel(ISpotifyClient spotifyClient, ILoginService loginService)
        {
            _spotifyClient = spotifyClient;
            _loginService = loginService;
            Name = "notset";
            Task.Run(async () => await Populate());
        }

        public async Task Populate()
        {
            var me = await _spotifyClient.GetMe("Bearer " + _loginService.AuthToken);
            Name = me.displayName;
            var rawPlayLists = await _spotifyClient.GetPlayLists("Bearer " + _loginService.AuthToken);

        }

    }
   
}
