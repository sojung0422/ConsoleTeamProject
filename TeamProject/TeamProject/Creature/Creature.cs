using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class Creature {
        #region Properties
        // 기본 정보: 게임 내내 변하지 않는 정보입니다.
        public string Name { get; set; } = "232";
        public float DefaultHpMax { get; set; }
        public float DefaultDamage { get; set; }
        public float DefaultDefense { get; set; }
        public float DefaultMpMax { get; set; }
        public float DefaultCritical { get; set; }
        public float DefaultAvoid { get; set; }
        
        // 현재 정보: 게임 중 변할 수 있는 정보입니다.
        public int Level { get; set; }
        public virtual float Hp {
            get => hp;
            set {
                if (value <= 0) hp = 0;
                else if (value >= DefaultHpMax) hp = DefaultHpMax;
                else hp = value;
            }
        }
        #endregion

        public float hp;
        public int textdelay = 200; // 텍스트 0.2초만에 한줄 출력

        public virtual void Attack(Creature creature, int line) {  }
        
        public virtual void OnDamaged(int damage) { }

        public virtual bool IsDead() {
            return false;
        }
    }
}
