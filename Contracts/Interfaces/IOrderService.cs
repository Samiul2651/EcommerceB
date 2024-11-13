using Contracts.Models;

namespace Contracts.Interfaces
{
    public interface IOrderService
    {
        public void SubmitOrder(Order order);
    }
}
