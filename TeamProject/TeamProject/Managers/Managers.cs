using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public static class Managers {
        private static SceneManager scene = new();
        public static SceneManager Scene => scene;
    }
}
