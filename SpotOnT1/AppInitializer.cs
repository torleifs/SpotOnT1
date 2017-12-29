using System;
using System.Threading.Tasks;
using SpotOnT1.Login;
namespace SpotOnT1
{
    public class AppInitializer
    {
        ILoginService _loginService;
        public AppInitializer(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task StartInitialization() {
            await _loginService.Initialize();
        }
    }
}
