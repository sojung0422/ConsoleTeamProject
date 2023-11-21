using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Console;

namespace TeamProject
{
    public class ConsumeItem : Item
    {
        public ConsumeItem() 
        {
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }

        public ConsumeItem(int id, string name, string description, int price, int stackCount, ItemType itemType = ItemType.ConsumeItem) : base(id, name, description, price, itemType, stackCount)
        {
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }

        public ConsumeItem(ConsumeItem reference) : base(reference) 
        {
            OnAdded += MergeItem;
            OnUsed += UseEffect;
        }

        public void MergeItem(Character onwer, Item duplicatedItem)
        {
            // 중복된 아이템이 있을 경우 duplicatedItem로 받아옵니다.
            // duplicatedItem이 null이 아니라면 두 아이템의 개수를 합칩니다.
            StackCount += duplicatedItem is ConsumeItem consume ? consume.StackCount : 0;
        }

        public virtual void UseEffect(Character owner)
        {
            if (StackCount > 1)
                StackCount--;
            else
                Game.Player.Inventory.Remove(this);
        }
        public override Item DeepCopy() => new ConsumeItem(this);
    }

    public class HealingPotion : ConsumeItem
    {
        public int healValue;

        public HealingPotion() : base()
        {
        }

        public HealingPotion(int id, string name, string description, int price, int stackCount, int healValue, ItemType itemType = ItemType.ConsumeItem) : base(id, name, description, price, stackCount, itemType)
        {
            this.healValue = healValue;
        }

        public HealingPotion(HealingPotion reference) : base(reference)
        {
            healValue = reference.healValue;
        }

        public override void UseEffect(Character owner)
        {
            base.UseEffect(owner);
            //owner.OnDamaged(-healValue);
            owner.Hp += healValue;
        }

        public override Item DeepCopy() => new HealingPotion(this);
    }
}
