using System;
using UnityEngine;

namespace AG.CustomLayout
{
    public interface ICustomLayoutItem
    {
        public Action<Transform> OnMove { get; set; }
        public CustomLayoutSlotData BaseSlot {get; set;}
    }
}