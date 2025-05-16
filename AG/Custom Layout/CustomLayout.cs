using System.Collections.Generic;
using AG.Base.Util;
using UnityEngine;

namespace AG.CustomLayout
{
    /// <summary>
    /// Custom Layout Component Changes Position And Rotation Of Layout Items Without Transform Component
    /// So We Can Achieve Smooth Move Transition On Item Order Change
    /// Be Able To Place Items On Curved Line With Different Item Rotation Values
    /// </summary>
    public class CustomLayout<TLayoutItem> : MonoBehaviour where TLayoutItem : MonoBehaviour, ICustomLayoutItem
    {
        [Header("Layout Settings")]
        [SerializeField] private float _itemWidth; //Item Gap Can Be Achievable By Correctly Adjusting This Value
        [Range(0, 45)][SerializeField] private float _itemRotationValue; //Item Rotation Values Towards Borders
        [Range(0, 45)][SerializeField] private float _curveValue; //Items Curve Value (Best Match --> _itemRotationValue = _curveValue)
        [Min(0)][SerializeField] private float _gapFromEachSide;

        private CustomLayoutSlotData[] _slotDatas; //Items Base Slot
        private List<TLayoutItem> _items = new();

        private float _width;
        private float _height;

        private void Awake()
        {
            CacheDimensions(GetComponent<RectTransform>().rect);
        }

        private void CacheDimensions(Rect rect)
        {
            _width = Mathf.Max(1, Mathf.Abs(rect.width) - Mathf.Clamp(Mathf.Abs(_itemWidth + (_gapFromEachSide * 2)), 0, Mathf.Abs(rect.width)));
            _height = Mathf.Max(1, rect.height); //Be Able To Calculate Correctly And Avoiding Errors By Setting Min Value 1
        }

        public void AddItem(TLayoutItem item)
        {
            _items.Add(item);
            item.OnMove += OnItemMove;
            UpdateLayout();
        }

        public void RemoveItem(TLayoutItem item)
        {
            if (_items.Remove(item))
            {
                item.OnMove -= OnItemMove;
                UpdateLayout();
            }
        }

        private void OnItemMove(Transform movingItem)
        {
            int movingItemIndex = movingItem.GetSiblingIndex();

            float currentDistance = GetDistanceToSlot(movingItem, movingItemIndex);
            int closestItemIndex = movingItemIndex;

            for (int i = 0; i < _slotDatas.Length; i++)
            {
                float minDistance = GetDistanceToSlot(movingItem, i);
                if (minDistance < currentDistance)
                {
                    currentDistance = minDistance;
                    closestItemIndex = i;
                }
            }

            if (movingItemIndex == closestItemIndex) return;

            MoveItems(movingItemIndex, closestItemIndex); //Move Current Item To Target Slot And Items On The Way To Their New Slot
        }

        private float GetDistanceToSlot(Transform item, int index)
        {
            Vector2 distanceToSlot = item.position - _slotDatas[index].position;
            return Mathf.Abs(Helper.GetPositionDifferenceByRotation(transform.eulerAngles.z, distanceToSlot.x, distanceToSlot.y));
        }

        private void MoveItems(int indexFrom, int indexTo)
        {
            int step = indexFrom < indexTo ? 1 : -1;
            for (int i = indexFrom; i != indexTo; i += step) //Moves All Items Between The Given Indices And Swaps Them Accordingly
            {
                Swap(i, i + step);
            }
        }

        private void Swap(int indexFrom, int indexTo) //Swap Items
        {
            _items[indexFrom].transform.SetSiblingIndex(indexTo);

            (_items[indexFrom], _items[indexTo]) = (_items[indexTo], _items[indexFrom]);

            _items[indexFrom].BaseSlot = _slotDatas[indexFrom];
            _items[indexTo].BaseSlot = _slotDatas[indexTo];
        }

        private void UpdateLayout()
        {
            _slotDatas = new CustomLayoutSlotData[_items.Count];

            //Adjust Values If Max Values Is Bigger Than Borders
            _itemWidth = _itemWidth.AdjustValue(GetMaxWidthValue, _width / 2);
            _itemRotationValue = _itemRotationValue.AdjustValue(GetMaxRotationValue, 90);
            _curveValue = _curveValue.AdjustValue(GetMaxCurveValue, _height / 2);

            for (int i = 0; i < _items.Count; i++)
            {
                _slotDatas[i] = CreateSlotData(i);
                _items[i].BaseSlot = _slotDatas[i];
            }
        }

        private float GetMaxWidthValue(float itemWidthValue)
        {
            return itemWidthValue.GetSequenceValue(_items.Count);
        }

        private float GetMaxRotationValue(float rotationValue)
        {
            return Mathf.Abs(rotationValue.GetSequenceValue(_items.Count));
        }

        private float GetMaxCurveValue(float curveValue)
        {
            return Mathf.Abs(curveValue.GetSequenceValue(_items.Count).GetSequenceValue(_items.Count));
        }

        private CustomLayoutSlotData CreateSlotData(int index)
        {
            float zRotation = -_itemRotationValue.GetSequenceValueByItemIndex(index, _items.Count);
            float xPosition = _itemWidth.GetSequenceValueByItemIndex(index, _items.Count);
            float yPosition = -Mathf.Abs(_curveValue.GetSequenceValueByItemIndex(index, _items.Count).GetSequenceValueByItemIndex(index, _items.Count));

            Vector3 position = transform.TransformPoint(new(xPosition, yPosition, 0));
            Vector3 rotation = new(0, 0, zRotation);

            return new(position, rotation);
        }
    }
}