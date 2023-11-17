using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class Creature {
        #region Properties
        // 기본 정보: 게임 내내 변하지 않는 정보입니다.
        public string Name { get; protected set; }
        public float DefaultHpMax { get; protected set; }
        public float DefaultDamage { get; protected set; }
        public float DefaultDefense { get; protected set; }
        public float DefaultMpMax { get; protected set; }
        public float DefaultCritical { get; protected set; }
        public float DefaultAvoid { get; protected set; }
        // 현재 정보: 게임 중 변할 수 있는 정보입니다.
        public int Level { get; protected set; }
        public virtual float Hp {
            get => hp;
            set {
                if (value <= 0) hp = 0;
                else if (value >= DefaultHpMax) hp = DefaultHpMax;
                else hp = value;
            }
        }
        #endregion

        protected float hp;

        public virtual void Attack(Creature creature) { }
        
        public virtual void OnDamaged(float damage) { }

        public virtual bool IsDead() {
            return false;
        }
    }
}
