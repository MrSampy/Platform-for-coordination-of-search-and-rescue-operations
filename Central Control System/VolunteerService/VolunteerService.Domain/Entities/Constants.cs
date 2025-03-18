namespace VolunteerService.Domain.Entities
{
    public static class Constants
    {
        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string DefaultException = "An unexpected error occurred. Please try again later.";

        public static readonly string AuthService = "AuthService";

        public static readonly string AuthServiceInMemory = "AuthServiceInMemory";
    }
}
