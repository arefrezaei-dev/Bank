using MediatR;

namespace Bank.Api.Common.Queries
{
    public record CustomerArchiveItem
    {
        public CustomerArchiveItem(Guid id, string firstname, string lastname)
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
        }
        public Guid Id { get; }
        public string Firstname { get; }
        public string Lastname { get; }
    }
    public class CustomersArchive : IRequest<IEnumerable<CustomerArchiveItem>> { }
}
