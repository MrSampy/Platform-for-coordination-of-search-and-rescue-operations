namespace UtilsService.Domain.Entities
{
    public static class Constants
    {
        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string AlreadyExistsSuchCombinationOfResourceUnitException = "Such combination of resource and unit already exists.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string AuthService = "AuthService";
    }
}
