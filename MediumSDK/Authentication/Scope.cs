namespace MediumSDK.Authentication
{
    public enum Scope
    {
        /// <summary>
        /// Grants basic access to a user’s profile (not including their email).
        /// </summary>
        BasicProfile,

        /// <summary>
        /// Grants the ability to list publications related to the user.
        /// </summary>
        ListPublications,

        /// <summary>
        /// Grants the ability to publish a post to the user’s profile.
        /// </summary>
        PublishPost,

        /// <summary>
        /// Grants the ability to upload an image for use within a Medium post.
        /// <para>This is an extended scope, need explicit prior permission from Medium.</para>
        /// </summary>
        UploadImage,
    }
}
