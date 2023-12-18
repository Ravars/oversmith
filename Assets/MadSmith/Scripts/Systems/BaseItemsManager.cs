using System.Linq;
using MadSmith.Scripts.Items;
using MadSmith.Scripts.Utils;
using UnityEngine;

namespace MadSmith.Scripts.Systems
{
    public class BaseItemsManager: PersistentSingleton<BaseItemsManager>
    {
        public BaseItem[] baseItems;

        public BaseItem GetBaseItemById(int id)
        {
            return baseItems.FirstOrDefault(x => x.id == id);
        }
    }
}