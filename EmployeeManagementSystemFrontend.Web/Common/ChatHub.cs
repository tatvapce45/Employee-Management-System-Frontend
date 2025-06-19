using Microsoft.AspNetCore.SignalR;

namespace EmployeeManagementSystemFrontend.Web.Common
{
    public class OrderItemDto
    {
        public string? UniqueId { get; set; }
        public int Orderid { get; set; }
    }

    public class ChatHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendOrderItems(List<OrderItemDto> items)
        {
            await Clients.All.SendAsync("ReceiveOrderItems", items);
        }
    }

}