namespace Identity.API.Models.Interfaces
{
    public interface IAuditInfo
    {
        DateTime CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }
    }
}
