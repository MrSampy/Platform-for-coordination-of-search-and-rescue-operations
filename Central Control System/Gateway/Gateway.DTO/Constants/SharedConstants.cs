namespace Gateway.DTO.Constants
{
    public static class SharedConstants
    {
        public static readonly string FieldIsRequierdException = "{0} is required.";

        public const string InvalidEmailFormat = "Invalid email format.";

        public const string InvalidBirthDate = "Birth date must be in the past.";

        public static readonly string InvalidFieldFormatException = "Invalid {0} format.";

        public static readonly string NotFoundEntityException = "No {0} with such gid: {1}.";

        public static readonly string InvalidPaginationQueryParametersException = "Page size and page number can not be less or equal to zero.";

        public static readonly string AlreadyExistsSuchCombinationOfResourceUnitException = "Such combination of resource and unit already exists.";

        public static readonly string EmptyResponseException = "Response is empty.";

        public static readonly string DefaultException = "An unexpected error occurred. Please try again later.";

        public static readonly string AuthService = "AuthService";

        public static readonly string UtilsService = "UtilsService";

        public static readonly string VolunteerService = "VolunteerService";

        public static readonly string OperatrionsService = "OperatrionsService";

        public static readonly string AuthServiceInMemory = "AuthServiceInMemory";

        public static readonly string UtilsServiceInMemory = "UtilsServiceInMemory";

        public static readonly string VolunteerServiceInMemory = "VolunteerServiceInMemory";

        public static readonly string OperatrionsServiceInMemory = "OperatrionsServiceInMemory";
    }
}
