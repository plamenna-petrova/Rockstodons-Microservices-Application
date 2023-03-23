namespace Catalog.API.Data.Data.Common.Models.Interfaces
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
