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
        public Character Parent { get; } //추후에 상점이나 몬스터도 인벤토리를 가질 수 있음.

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
            //onRemoved += item => { /* MaxPad 갱신 */ };
            //event를 이용해서 아이템이 삽입/삭제될 때마다 player data를 저장하게 만들 수도 있음.
        }

        public void Add(Item item)
        {
            if (!HasSameItem(item, out Item res))
            {
                // 중복되는 아이템이 없는 경우만 add
                items.Add(item);
                item.OnAdd(Parent);
            }
            else if (res.StackCount.HasValue)
            {
                // 개수를 쌓을 수 있는 아이템이라면
                // 인벤토리에 있는 소모품의 stackCnt에 item의 stackCnt를 더함
                res.OnAdd(Parent, item);
            }
            onAdded?.Invoke(item);
        }

        public void Remove(Item item)
        {
            items.Remove(item);
            item.OnRemove(Parent);
            onRemoved?.Invoke(item);
        }

        /// <summary>
        /// 인벤토리에 같은 아이템이 있는지 찾습니다.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool HasSameItem(Item item) => HasSameItem(item, out _);

        /// <summary>
        /// 인벤토리에 같은 아이템이 있는지 찾습니다. <br/>
        /// 중복되는 아이템이 있다면 out 매개변수로 반환합니다.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="res"> item과 같은 아이템의 객체입니다.</param>
        /// <returns></returns>
        public bool HasSameItem(Item item, out Item res)
        {
            for (int i = 0; i < items.Count; i++)
            {

                if (items[i].ID == item.ID)
                {
                    res = items[i];
                    return true;
                }
            }
            res = null;
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
