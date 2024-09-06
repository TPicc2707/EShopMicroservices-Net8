using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.UpdateOrder;
public class UpdateOrderHandler(IApplicationDbContext dbcontext)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        //Update Order entity from command object
        //save to database
        //return result

        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbcontext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);
        if (order is null)
            throw new OrderNotFoundException(command.Order.Id);

        await UpdateOrderWithNewValues(order, command.Order);

        dbcontext.Orders.Update(order);
        await dbcontext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);

    }

    private async Task UpdateOrderWithNewValues(Order order, OrderDto orderDto)
    {
        var updatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
        var updatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
        var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
        var orderItems = await dbcontext.OrderItems.Where(o => o.OrderId == order.Id).ToListAsync();
        //dbcontext.OrderItems.RemoveRange(orderItems);

        order.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: updatedShippingAddress,
            billingAddress: updatedBillingAddress,
            payment: updatedPayment,
            status: orderDto.Status);

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            order.Remove(ProductId.Of(orderItemDto.ProductId));
            order.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
        }
    }
}

