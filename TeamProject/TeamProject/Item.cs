using static System.Console;
namespace TeamProject
{
    // 아이템 타입
    public enum ItemType
    {
        Gear,
        ConsumeItem,
    }

    public class Item
    {
        /// <summary>
        /// 아이템을 식별할 고유 번호 <br/>
        /// </summary>
        public int ID { get; protected set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public int Price { get; protected set; }
        public ItemType Type { get; protected set; }

        /// <summary>
        /// StackCount.HasValue가 null이라면 쌓을 수 없는 아이템
        /// </summary>
        public int? StackCount { get; set; }

        public Item(int id, string name, string description, int price, ItemType type, int? stackCount = null)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            Type = type;
            StackCount = stackCount;
        }

        public Item(Item reference)
        {
            ID = reference.ID;
            Name = reference.Name;
            Description = reference.Description;
            Price = reference.Price;
            Type = reference.Type;
            StackCount = reference.StackCount;
        }

        //아이템이 사용될 때 호출될 이벤트
        //장비라면 장착, 소모품이라면 사용되고 stack cnt -1
        protected event Action<Character>? OnUsed;

        //아이템이 인벤토리에 추가될 때 호출될 이벤트
        //소모품의 경우 인벤토리에 같은 아이템이 있다면 새로운 칸에 들어가는게 아니라, 그 칸의 stack cnt를 +1 해야합니다.
        protected event Action<Character, Item>? OnAdded;

        //아이템이 인벤토리에서 삭제될 떄 호출될 이벤트
        //만약 장착중인 장비아이템이 삭제된다면, 장착해제도 같이 진행해야합니다.
        protected event Action<Character>? OnRemoved;

        public void Use(Character owner) => OnUsed?.Invoke(owner);
        public void OnAdd(Character owner, Item duplicatedItem = null) => OnAdded?.Invoke(owner, duplicatedItem);
        public void OnRemove(Character owner) => OnRemoved?.Invoke(owner);
        public virtual Item DeepCopy() => new Item(this);
    }
}