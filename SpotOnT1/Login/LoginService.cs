using System;
using System.Threading.Tasks;

using Xamarin.Auth;

namespace SpotOnT1.Login
{
    public interface ILoginService {
        Task Login();
        string AuthToken { get; set; }
    }
    public class LoginService : ILoginService
    {
        private OAuth2Authenticator authenticator;
        private TaskCompletionSource<bool> loginCompletionSource;
        private string authToken;
        public string AuthToken { get { return authToken; } set { authToken = value; } }
        private ISpotifyClient _spotifyClient;
       
        public LoginService(ISpotifyClient spotifyClient) 
        {
            _spotifyClient = spotifyClient;
        }

        public async Task Login() {
            var ss = Constants.iOSClientId;
            authenticator = new OAuth2Authenticator(
                clientId: Constants.iOSClientId,
                scope: Constants.Scopes,
                authorizeUrl: new Uri(Constants.AuthEndpoint),
                redirectUrl: new Uri(Constants.iOSRedirectUri),
                isUsingNativeUI: false);
            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            loginCompletionSource = new TaskCompletionSource<bool>();
            presenter.Login(authenticator);
            await loginCompletionSource.Task;
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            if (authenticator.IsAuthenticated())
            {
                AccountStore store = AccountStore.Create();
                await store.SaveAsync(e.Account, "SpotAccount");
            }
            string token = e.Account.Properties["access_token"];
            AuthToken = token;
            loginCompletionSource.TrySetResult(true);

           // var user = await _spotifyClient.GetMe("Bearer " + token);
           // var playlists = await _spotifyClient.GetPlayLists("Bearer " + token);
        }
        void OnAuthError(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }
            loginCompletionSource.TrySetResult(false);
            //  Console.WriteLine("Authentication error: " + e.Message);
        }
    }
}
