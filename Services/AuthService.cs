using Microsoft.AspNetCore.Components.Authorization;

namespace EventEaseF.Services
{
    public class AuthService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        public AuthService(AuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        public async Task<bool> IsAdminAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.IsInRole("Administrator");
        }
    }
}
