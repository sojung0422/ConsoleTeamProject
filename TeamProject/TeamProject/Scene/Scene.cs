using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public class Scene {
        public virtual string Title { get; protected set; }

        protected List<ActionOption> Options { get; set; } = new();
        /// <summary>
        /// 씬 진입 시 액션.
        /// </summary>
        public virtual void EnterScene() {

        }
        /// <summary>
        /// 다음 씬으로 넘어가기 위한 조건.
        /// </summary>
        public virtual void NextScene() {

        }
        /// <summary>
        /// 씬 보여주기.
        /// </summary>
        protected virtual void DrawScene() {

        }
    }
}
