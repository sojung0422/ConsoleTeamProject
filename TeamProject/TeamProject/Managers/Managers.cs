using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {
    public static class Managers {
        private static SceneManager scene = new();
        private static GameManager game = new();
        private static TableManager table = new();
        public static SceneManager Scene => scene;
        public static GameManager Game => game;
        public static TableManager Table => table;
    }
}
