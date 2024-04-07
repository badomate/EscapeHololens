using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameplay.Items.Properties
{
    // Used to update the item's properties
    public interface IItemUpdatable
    {
        public void UpdateItem(GameObject item = null);
    }
}
