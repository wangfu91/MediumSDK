using MediumSDK.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MediumSDK
{
    public class ApiClient : ClientBase
    {
        public async Task<User> GetCurrentUserAsync(string accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/me"))
            {
                return (await SendRequestWithTokenAsync<DataContainer<User>>(request, accessToken)).Data;
            }
        }

        public async Task<IEnumerable<Publication>> GetPublicationsAsync(string userId, string accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + $"/users/{userId}/publications"))
            {
                return (await SendRequestWithTokenAsync<DataContainer<IEnumerable<Publication>>>(request, accessToken)).Data;
            }
        }

        public async Task<IEnumerable<Contributor>> GetContributorsAsync(string publicationId, string accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, BaseUrl + $"/publications/{publicationId}/contributors"))
            {
                return (await SendRequestWithTokenAsync<DataContainer<IEnumerable<Contributor>>>(request, accessToken)).Data;
            }
        }

        public async Task<Post> CreatePostAsync(string authorId, CreatePostRequestBody createPostRequestBody, string accessToken)
        {
            return await CreatePostInternalAsync($"/users/{authorId}/posts", createPostRequestBody, accessToken);
        }

        public async Task<Post> CreatePostUnderPublicationAsync(string publicationId, CreatePostRequestBody createPostRequestBody, string accessToken)
        {
            return await CreatePostInternalAsync($"/publications/{publicationId}/posts", createPostRequestBody, accessToken);
        }

        private async Task<Post> CreatePostInternalAsync(string endpointUrl, CreatePostRequestBody createPostRequestBody, string accessToken)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + endpointUrl))
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(new
                {
                    title = createPostRequestBody.Title,
                    contentFormat = createPostRequestBody.ContentFormat.ToString().ToLowerInvariant(),
                    content = createPostRequestBody.Content,
                    tags = createPostRequestBody.Tags,
                    canonicalUrl = createPostRequestBody.CanonicalUrl,
                    publishStatus = createPostRequestBody.PublishStatus.ToString().ToLowerInvariant(),
                    license = createPostRequestBody.License.ToString(),
                    publishedAt = createPostRequestBody.PublishedAt,
                    notifyFollowers = createPostRequestBody.NotifyFollowers,
                }));
                stringContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                request.Content = stringContent;
                return (await SendRequestWithTokenAsync<DataContainer<Post>>(request, accessToken)).Data;
            }
        }

        public async Task<Image> UploadImageAsync(UploadImageRequestBody uploadImageRequestBody, string accessToken, string boundary = null)
        {
            if (string.IsNullOrWhiteSpace(boundary))
            {
                boundary = Guid.NewGuid().ToString("N");
            }

            using (var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + "/images"))
            {
                var requestContent = new MultipartFormDataContent($"--{boundary}");
                var imageContent = new ByteArrayContent(uploadImageRequestBody.ContentBytes);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse(uploadImageRequestBody.ContentType);
                requestContent.Add(imageContent, "image", "image");
                request.Content = requestContent;

                return (await SendRequestWithTokenAsync<DataContainer<Image>>(request, accessToken)).Data;
            }
        }
    }
}
