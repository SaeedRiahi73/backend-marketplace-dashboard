namespace Task_Application.Enums;

public enum ProductFilterStatus
{
    All = 0,
    Active = 1,
    DeActive = 2,

    // This is a calculated filter based on Quantity, not a value stored in Product.Status.
    OutOfStock = 3
}
