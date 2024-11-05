using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IOrderService
    {
        public bool SubmitOrder(Order order);
    }
}
