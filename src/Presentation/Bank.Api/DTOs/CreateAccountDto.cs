using System.ComponentModel.DataAnnotations;

namespace Bank.Api.DTOs
{
    public class CreateAccountDto
    {
        [Required]
        public string CurrencyCode { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
    }
}
