using MediumSDK;
using MediumSDK.Authentication;
using System;
using System.Net;
using Xunit;
using System.Threading.Tasks;

namespace SdkTests
{
    public class OAuthClientTests
    {
        private const string FakeClientId = "f1234f123456";
        private const string FakeClientSecret = "abcd12abcdef12345678901234abcdefgabcdef";

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData(" ", null)]
        [InlineData(FakeClientId, null)]
        [InlineData(FakeClientId, "")]
        [InlineData(FakeClientId, " ")]
        public void OAuthClientCtor_InvalidParameter_ShouldThrowException(string clientId, string clientSecret)
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                var oAuthClient = new OAuthClient(clientId, clientSecret);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetAuthorizeUrl_InvalidRedirectUrl_ShouldThrowException(string redirectUrl)
        {
            Assert.Throws(typeof(ArgumentException), () =>
            {
                var oAuthClient = new OAuthClient(FakeClientId, FakeClientSecret);
                oAuthClient.GetAuthorizeUrl("state", redirectUrl, new[]
                {
                    Scope.BasicProfile,
                    Scope.PublishPost,
                    Scope.ListPublications,
                });
            });
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", null)]
        [InlineData("qwerty", null)]
        [InlineData("qwerty", "")]
        public async Task GetAccessToken_InvalidCode_ShouldThrowException(string code, string redirdctUrl)
        {
            await Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                var oAuthClient = new OAuthClient(FakeClientId, FakeClientSecret);
                await oAuthClient.GetAccessTokenAsync(code, redirdctUrl);
            });
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAccessToken_InvalidRefreshToken_ShouldThrowException(string refreshToken)
        {
            await Assert.ThrowsAsync(typeof(ArgumentException), async () =>
            {
                var oAuthClient = new OAuthClient(FakeClientId, FakeClientSecret);
                await oAuthClient.GetAccessTokenAsync(refreshToken);
            });
        }

        [Fact]
        public void GetAuthorizeUrl_ShouldReturn_CorrectFormat()
        {
            const string state = "state";
            const string redirectUrl = "http://example.com";
            var scope = new[]
            {
                Scope.BasicProfile,
                Scope.PublishPost
            };

            var oAuthClient = new OAuthClient(FakeClientId, FakeClientSecret);

            var result = oAuthClient.GetAuthorizeUrl(state, redirectUrl, scope);

            var expect = "https://medium.com/m/oauth/authorize?" +
                         $"client_id={FakeClientId}&" +
                         $"scope={WebUtility.UrlEncode("basicProfile,publishPost")}&" +
                         $"state={state}&" +
                         "response_type=code&" +
                         $"redirect_uri={WebUtility.UrlEncode(redirectUrl)}";

            Assert.Equal(expect, result);
        }
    }
}
