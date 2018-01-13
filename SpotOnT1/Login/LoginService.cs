using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Auth;

namespace SpotOnT1.Login
{
    public interface ILoginService {
        Task Initialize();
        Task Login();
        string AuthToken { get; set; }
        bool IsLoggedIn { get; set; }
    }
    public class LoginService : ILoginService
    {
        private const string ACCOUNTSTORE_KEY = "SpotAccount";
        private OAuth2Authenticator authenticator;
        private TaskCompletionSource<bool> loginCompletionSource;
        private string authToken;
        private bool isLoggedIn = false;
        private ISpotifyClient _spotifyClient;
        private AccountStore _accountStore;

        public string AuthToken { 
            get => authToken; 
            set => authToken = value;
        }
        public bool IsLoggedIn { 
            get => isLoggedIn; 
            set => isLoggedIn = value;
        }
       
        public LoginService(ISpotifyClient spotifyClient) 
        {
            _spotifyClient = spotifyClient;
            _accountStore = AccountStore.Create();
        }

        public async Task Initialize() {
            var accounts = await _accountStore.FindAccountsForServiceAsync(ACCOUNTSTORE_KEY);
            var account = accounts.FirstOrDefault();
            if (account != null)
            {
                isLoggedIn = GetLoginStatus(account);
                if (isLoggedIn)
                {
                    AuthToken = account.Properties["access_token"];
                }
            } else {
                isLoggedIn = false;
            }
        }

        private bool GetLoginStatus(Account account) {
            string createdString = account.Properties["created"];
            string expires = account.Properties["expires_in"];
            int secondsToExpire = int.Parse(expires);
            var createdTime = DateTimeOffset.Parse(createdString);
            DateTimeOffset expireDate = createdTime.AddSeconds(secondsToExpire);
            return (DateTimeOffset.Now < expireDate);
        }

        public async Task Login() {
            Debug.WriteLine("Logging in");
            var ss = Constants.iOSClientId;
            authenticator = new OAuth2Authenticator(
                clientId: Constants.iOSClientId,
                scope: Constants.Scopes,
                authorizeUrl: new Uri(Constants.AuthEndpoint),
                redirectUrl: new Uri(Constants.iOSRedirectUri),
                isUsingNativeUI: false);
            Debug.WriteLine("Got authenticator");
            authenticator.Completed += OnAuthCompleted;
            authenticator.Error += OnAuthError;
            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();

            loginCompletionSource = new TaskCompletionSource<bool>();
            presenter.Login(authenticator);
            await loginCompletionSource.Task;
        }

        async void OnAuthCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            Debug.WriteLine("Auth completed");
            var now = DateTimeOffset.Now; 
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= OnAuthCompleted;
                authenticator.Error -= OnAuthError;
            }

                
            e.Account.Properties["created"] = now.ToString();
            await _accountStore.SaveAsync(e.Account, "SpotAccount");

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
