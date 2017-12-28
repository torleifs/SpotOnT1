using System;
using System.Threading.Tasks;
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
        }

    }
   
}
