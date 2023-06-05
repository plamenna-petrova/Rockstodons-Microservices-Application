namespace Catalog.API.Common
{
    public class GlobalConstants
    {
        // API Messages

        public const string EntitiesNotFoundResult = "No {0} were found";

        public const string GetAllEntitiesExceptionMessage = "Something went wrong when retrieving the {0} \n {1}";

        public const string GetAllEntitiesWithDeletedRecordsExceptionMessage = "Something went wrong, " +
            "when trying to retrieve all the {0}, including the deleted records \n {1}";

        public const string InternalServerErrorMessage = "Internal Server Error";

        public const string EntityByIdNotFoundResult = "The {0} with id {1} couldn't be found";

        public const string GetEntityByIdExceptionMessage = "Something went wrong when retrieving the " +
            "{0} with an id {1} \n {2}";

        public const string GetEntityDetailsExceptionMessage = "Something went wrong when retrieving the " +
            "{0} details with an id {1} \n {2}";

        public const string InvalidObjectForEntityCreation = "The {0} object, sent from the client is null";

        public const string BadRequestMessage = "The {0} {1} object is null";

        public const string InvalidObjectForEntityUpdate = "The {0} object for update, sent from the client, is null";

        public const string InvalidObjectForEntityPatch = "The {0} object for patch, sent from the client, is null";

        public const string EntityCreationExceptionMessage = "Something went wrong when trying to create a {0} \n {1}";

        public const string EntityUpdateExceptionMessage = "Something went wrong when trying to update a {0} \n {1}";

        public const string EntityDeletionExceptionMessage = "Something went wrong when trying to delete the " +
            "{0} with provided id {1} {2}";

        public const string EntityHardDeletionExceptionMessage = "Something went wrong when trying to delete the " +
            "{0} completely with provided id {1} {2}";

        public const string EntityRestoreExceptionMessage = "Something went wrong when trying to restore the " +
            "{0} with provided id {1} {2}";

        // Roles

        public const string AdministratorRoleName = "STSAdminRole";

        public const string EditorRoleName = "Editor";

        public const string NormalUserRoleName = "User";

        public const string RolesDelimeter = ",";
    }
}
