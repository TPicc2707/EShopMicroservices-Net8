namespace Ordering.Infrastructure.Data.Extensions;
internal class InitialData
{
    public static IEnumerable<Customer> Customers =>
    new List<Customer>
    {
        Customer.Create(CustomerId.Of(new Guid("ac0d304a-d5fa-486a-a20f-a5763547b742")), "tony", "tony@gmail.com"),
        Customer.Create(CustomerId.Of(new Guid("85bbe2c5-bde8-4c2d-9513-89e3eb455983")), "john", "john@gmail.com")
    };

    public static IEnumerable<Product> Products =>
    new List<Product>
    {
        Product.Create(ProductId.Of(new Guid("d8bd14ea-efd4-48a0-9e53-321e39681201")), "IPhone X", 500),
        Product.Create(ProductId.Of(new Guid("7dc0013c-f09b-4e47-b4dc-b63161f8f055")), "Samsung 10", 400),
        Product.Create(ProductId.Of(new Guid("063770fb-5651-49bd-a04e-74687df1bd57")), "Huawei Plus", 650),
        Product.Create(ProductId.Of(new Guid("c9dcbacb-16e7-45e0-a496-b66ef212ac16")), "Xiaomi Mi", 450)
    };

    public static IEnumerable<Order> OrdersWithItems
    {
        get
        {
            var address1 = Address.Of("tony", "piccirilli", "tony@gmail.com", "123 Way Street", "US", "NY", "12345");
            var address2 = Address.Of("john", "doe", "john@gmail.com", "Broadway No:1", "US", "NY", "12346");

            var payment1 = Payment.Of("tony", "5555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("john", "8885555555554444", "06/30", "222", 2);

            var order1 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("ac0d304a-d5fa-486a-a20f-a5763547b742")),
                            OrderName.Of("ORD_1"),
                            shippingAddress: address1,
                            billingAddress: address1,
                            payment1);

            order1.Add(ProductId.Of(new Guid("7dc0013c-f09b-4e47-b4dc-b63161f8f055")), 2, 500);
            order1.Add(ProductId.Of(new Guid("063770fb-5651-49bd-a04e-74687df1bd57")), 1, 400);

            var order2 = Order.Create(
                            OrderId.Of(Guid.NewGuid()),
                            CustomerId.Of(new Guid("85bbe2c5-bde8-4c2d-9513-89e3eb455983")),
                            OrderName.Of("ORD_2"),
                            shippingAddress: address2,
                            billingAddress: address2,
                            payment2);

            order2.Add(ProductId.Of(new Guid("d8bd14ea-efd4-48a0-9e53-321e39681201")), 1, 650);
            order2.Add(ProductId.Of(new Guid("c9dcbacb-16e7-45e0-a496-b66ef212ac16")), 2, 450);

            return new List<Order> { order1, order2 }; 
        }
    }
}


