namespace Application.Services.HMS.Database;

public enum PantryOrderStatus
{
    Received = 0,
    Preparing = 1,
    ReadyToServe = 2,
    Served = 3,
    Cancelled = 4
}