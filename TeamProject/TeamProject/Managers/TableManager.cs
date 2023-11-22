using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject; 
public class TableManager {

    private Dictionary<string, TableFormatter<Item>> items = new();
    private Dictionary<string, TableFormatter<Character>> jobs = new();


    public List<TableFormatter<T>> GetFormatters<T>(string[] keys) {
        List<TableFormatter<T>> list = new();
        for (int i = 0; i < keys.Length; i++) {
            TableFormatter<T>? formatter = typeof(Item) == typeof(T) ? items[keys[i]] as TableFormatter<T> : jobs[keys[i]] as TableFormatter<T>;
            if (formatter == null) continue;
            list.Add(formatter);
        }
        return list;
    }

    public void Initialize() {
        items["Index"] = new("Index", "", 2);
        items["Name"] = new("Name", "이름", 22, item => {
            if (item is Gear gear) {
                if (gear.IsEquip) return $"[E] {gear.Name}";
                else return gear.Name;
            }
            else return item.Name;
        });
        items["StackCount"] = new("StackCount", "개수", 8, i => i.StackCount.HasValue ? $"{i.StackCount.Value} 개" : "");
        items["ItemType"] = new("ItemType", "타입", 15, item => {
            if (item is Gear gear) return gear.GearType.String();
            else return item.Type.String();
        });
        items["Effect"] = new("Effect", "효과", 34, item => {
            if (item is Gear gear) return gear.StatToString();
            else if (item is ConsumeItem consume) return consume.EffectDesc;
            else return string.Empty;
        });
        items["Desc"] = new("Desc", "설명", 30, item => item.Description);
        items["Cost"] = new("Cost", "비용", 10, item => item.Price.ToString());
        items["SellCost"] = new("SellCost", "비용", 10, item => ((int)(item.Price * 0.85f)).ToString());
        items["ShopCount"] = new("ShopCount", "보유 개수", 11, item => {
            if (Game.Player.Inventory.HasSameItem(item, out Item res))
                return res.StackCount.HasValue ? $"{res.StackCount.Value} 개" : "보유중";
            else
                return item.StackCount.HasValue ? "0 개" : "미보유";
        });

        jobs["Job"] = new("Job", "직업", 10, c => c.Job.ToString());
        jobs["Damage"] = new("DefaultDamage", "공격력", 10, c => c.DefaultDamage.ToString());
        jobs["Defense"] = new("DefaultDefense", "방어력", 10, c => c.DefaultDefense.ToString());
        jobs["HpMax"] = new("DefalutHpMax", "체 력", 10, c => c.DefaultHpMax.ToString());
        jobs["MpMax"] = new("DefalutMpMax", "마 나", 10, c => c.DefaultMpMax.ToString());
        jobs["Critical"] = new("Critical", "크리율", 20, c => c.Critical.ToString("0%"));
        jobs["Avoid"] = new("Avoid", "회피율", 20, c => c.Critical.ToString("0%"));
    }
}

public class TableFormatter<T> {
    public string key;
    public string description;
    public int length;
    public Func<T, string>? dataSelector;

    public TableFormatter(string key, string description, int length, Func<T, string>? dataSelector = null) {
        this.key = key;
        this.description = description;
        this.length = length;
        this.dataSelector = dataSelector;
    }

    public string GetTitle() => Renderer.GetTableElementString(length, description, true);
    public string GetLine() => new('=', length);
    public string GetString(int index) => Renderer.GetTableElementString(length, index.ToString(), false);
    public string GetString(T item) => Renderer.GetTableElementString(length, dataSelector(item), false);
}

