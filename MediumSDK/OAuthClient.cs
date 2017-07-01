using MediumSDK.Authentication;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MediumSDK.Utils;
using System;

namespace MediumSDK
{
    public class OAuthClient : ClientBase
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public OAuthClient(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId))
                throw new ArgumentException("Client id cannot be null or empty", nameof(clientId));

            if (string.IsNullOrWhiteSpace(clientSecret))
                throw new ArgumentException("Client secret cannot be null or empty", nameof(clientSecret));

            _clientId = clientId;
            _clientSecret = clientSecret;
        }

        public string GetAuthorizeUrl(
            string state,
            string redirectUrl,
            Scope[] scope)
        {
            if (string.IsNullOrWhiteSpace(state))
                state = "state";

            if (string.IsNullOrWhiteSpace(redirectUrl))
                throw new ArgumentException("RedirectUrl cannot be null or empty", nameof(redirectUrl));

            if (scope == null || scope.Length < 1)
                throw new ArgumentException("Should at least specify one value", nameof(scope));

            var paramSet = new Dictionary<string, string>
            {
                {"client_id", _clientId},
                {"scope", scope.Select(s => s.ToString().PascalCaseToCamelCase()).Aggregate((c, n) => $"{c},{n}")},
                {"state", state},
                {"response_type", "code"},
                {"redirect_uri", redirectUrl}
            };

            return BuildRequestUri("https://medium.com/m/oauth/authorize", paramSet).AbsoluteUri;
        }

        public async Task<Token> GetAccessTokenAsync(string code, string redirectUri)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Code cannot be null or empty", nameof(code));

            if (string.IsNullOrWhiteSpace(redirectUri))
                throw new ArgumentException("RedirectUrl cannot be null or empty", nameof(redirectUri));

            var postParams = new Dictionary<string, string>
            {
                {"code", code},
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"grant_type", "authorization_code"},
                {"redirect_uri", redirectUri}
            };

            return await GetAccessTokenAsync(postParams);
        }

        public async Task<Token> GetAccessTokenAsync(string refreshToken)
        {
            if(string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("RefreshToken cannot be null or empty", nameof(refreshToken));

            var postParams = new Dictionary<string, string>
            {
                {"refresh_token", refreshToken},
                {"client_id", _clientId},
                {"client_secret", _clientSecret},
                {"grant_type", "refresh_token"},
            };

            return await GetAccessTokenAsync(postParams);
        }

        private async Task<Token> GetAccessTokenAsync(Dictionary<string, string> postParams)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}/tokens"))
            {
                request.Content = new FormUrlEncodedContent(postParams);
                return await SendRequestAsync<Token>(request);
            }
        }

    }
}
