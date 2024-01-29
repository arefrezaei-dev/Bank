namespace Bank.Worker.Core
{
    record InfrastructureConfig(string EventBus, string AggregateStore, string QueryDb);
}
