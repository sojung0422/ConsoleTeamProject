using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject;

public class Skill
{
    public List<string> Names;
    public List<float> Damage;
    public List<int> MpCost;
    public int Count;

    public Skill(List<string> names, List<float> damage, List<int> mpCost)
    {
        Names = names;
        Damage = damage;
        Count = Names.Count;
        MpCost = mpCost;
    }
}

