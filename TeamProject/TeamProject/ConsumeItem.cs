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
        public ConsumeItem(int id, string name, string description, int price, int stackCount) : base(id, name, description, price, stackCount)
        {
            OnAdded += MergeItem;
        }

        public void MergeItem(Character onwer, Item duplicatedItem)
        {
            // 중복된 아이템이 있을 경우 duplicatedItem로 받아옵니다.
            // duplicatedItem이 null이 아니라면 두 아이템의 개수를 합칩니다.
            StackCount += duplicatedItem is ConsumeItem consume ? consume.StackCount : 0;
        }
    }
}
