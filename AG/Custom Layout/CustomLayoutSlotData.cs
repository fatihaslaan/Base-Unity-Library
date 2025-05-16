using UnityEngine;

namespace AG.CustomLayout
{
    public struct CustomLayoutSlotData
    {
        public Vector3 position;
        public Vector3 rotation;

        public CustomLayoutSlotData(Vector3 pos, Vector3 rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}