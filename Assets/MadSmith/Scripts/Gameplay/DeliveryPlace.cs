using MadSmith.Scripts.Items;
using MadSmith.Scripts.Managers;
using Mirror;

namespace MadSmith.Scripts.Gameplay
{
    public class DeliveryPlace : NetworkBehaviour
    {
        // public OrdersManager OrdersManager;

        public bool DeliverItem(BaseItem item)
        {
            return OrdersManager.Instance.CheckOrder(item);
        }
    }
}
