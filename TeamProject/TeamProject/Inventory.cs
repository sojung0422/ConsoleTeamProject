using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    public class Inventory
    {
        List<Item> items;
        /// <summary>
        /// 이 인벤토리의 주인인 Character 클래스를 가리킵니다.
        /// </summary>
        public Character Parent { get; }
        /// <summary>
        /// 인벤토리 내부의 아이템 개수를 반환합니다.<br/>
        /// return items.Count
        /// </summary>
        public int Count { get => items.Count; }

        Action<Item>? onAdded;
        Action<Item>? onRemoved;

        /// <summary>
        /// 현재 인벤토리에서 이름이 가장 긴 아이템의 byte를 저장합니다.<br/>
        /// 콘솔에서 테이블 크기를 결정하기 위해 사용합니다.
        /// </summary>
        public int MaxPad { get; private set; }
        public Inventory(Character _parent)
        {
            items = new();
            Parent = _parent;
            MaxPad = 4;
            //onAdded += item => MaxPad = Math.Max(MaxPad, Encoding.Default.GetBytes(item.GearName).Length);
            //onRemoved += item => { /* parent의 equipment에서 item을 unequip하게 만드는 이벤트 */ };
            //event를 이용해서 아이템이 삽입/삭제될 때마다 player data를 저장하게 만들 수도 있음.
        }

        public void Add(Item item)
        {
            items.Add(item);
            onAdded?.Invoke(item);
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            onRemoved?.Invoke(item);
        }

        public bool HasSameItem(Item item)
        {
            for (int i = 0; i < items.Count; i++)
            {
                //현재 이름으로 비교하지만 추후에 Item마다 고유번호를 추가하거나 하는 식으로도 비교 가능
                if (items[i].Name == item.Name)
                    return true;
            }
            return false;
        }

        public void OrderBy(ItemOrderMode mode)
        {
            switch (mode)
            {
                case ItemOrderMode.Default:
                    //items = items.OrderBy(element => element.ID).ToList();
                    break;
                case ItemOrderMode.NameAsc:
                    break;
                case ItemOrderMode.NameDesc:
                    break;
                case ItemOrderMode.AtkDesc:
                    break;
                case ItemOrderMode.DefDesc:
                    break;
                case ItemOrderMode.PriceAsc:
                    break;
                case ItemOrderMode.PriceDesc:
                    break;
            }
        }

        //List처럼 인덱싱.. ex) player.Inventory[0]
        public Item this[int idx]
        {
            get
            {
                if (idx < 0 || idx >= items.Count)
                    throw new ArgumentOutOfRangeException(nameof(idx));
                return items[idx];
            }
        }

        //추후에 data 저장 시에 사용할 메서드
        //public JArray ToJArray()
        //{
        //    JArray res = new JArray(items.Select(x => x.GearName).ToList());
        //    return res;
        //}
    }

    public enum ItemOrderMode
    {
        Default,
        NameAsc,
        NameDesc,
        AtkDesc,
        DefDesc,
        PriceAsc,
        PriceDesc,
    }
}
