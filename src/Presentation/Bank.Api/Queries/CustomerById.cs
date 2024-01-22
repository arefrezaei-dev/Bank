using Bank.Domain;
using MediatR;

namespace Bank.Api.Queries
{
    public record CustomerAccountDetails(Guid id, Money balance)
    {
        public static CustomerAccountDetails Map(Account account)
            => new CustomerAccountDetails(account.Id, account.Balance);
    }
    public record CustomerDetails
    {
        public CustomerDetails(Guid id, string firstname, string lastname, string email, CustomerAccountDetails[] accounts, Money totalBalance)
        {
            Id = id;
            FirstName = firstname;
            LastName = lastname;
            Email = email;
            Accounts = (accounts ?? Enumerable.Empty<CustomerAccountDetails>()).ToArray();
            TotalBalance = totalBalance;
        }
        public Guid Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public CustomerAccountDetails[] Accounts { get; init; }
        public Money TotalBalance { get; init; }
    }
    public record CustomerById(Guid customerId) : IRequest<CustomerDetails>;
}
