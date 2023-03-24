using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Utils.Attributes
{
    public class ValidDateTime : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            return dateTime >= DateTime.Now;
        }
    }
}
