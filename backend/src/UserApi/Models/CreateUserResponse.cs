namespace UserApi.Models
{
    /// <summary>
    /// Represents the response returned when a new user is created.
    /// </summary>
    public class CreateUserResponse
    {
        /// <summary>
        /// ID of the new user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The first name of the new user.
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// The last name of the new user.
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// The username of the new user.
        /// </summary>
        public string Username  { get; set; } = null!;

        /// <summary>
        /// The email address of the new user.
        /// </summary>
        public string Email { get; set; } = null!;
    }
}
