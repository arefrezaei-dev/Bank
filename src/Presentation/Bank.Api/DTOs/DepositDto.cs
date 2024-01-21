using System.ComponentModel.DataAnnotations;

namespace Bank.Api.DTOs
{
    public class DepositDto
    {
        [Required]
        public string CurrencyCode { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
