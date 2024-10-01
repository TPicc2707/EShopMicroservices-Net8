namespace Shopping.Web.Pages
{
    public class OrderListModel 
        (IOrderingService orderingService, ILogger<OrderListModel> logger)
        : PageModel
    {

        public IEnumerable<OrderModel> Orders { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            // assumption customerId is passed in from the UI authenticated user swn
            var customerId = new Guid("AC0D304A-D5FA-486A-A20F-A5763547B742");

            var response = await orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;

            return Page();
        }
    }
}
