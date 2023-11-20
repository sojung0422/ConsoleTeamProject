using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {

    public class GameData {

    }
    public class GameManager {

        private GameData data = new();
        private string path;

        public void Initialize() {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        }

        public void SaveGame() {
            string jsonStr = JsonConvert.SerializeObject(data);
            File.WriteAllText(path, jsonStr);
        }

        public bool LoadGame() {
            if (!File.Exists(path)) return false;

            string file = File.ReadAllText(path);
            GameData data = JsonConvert.DeserializeObject<GameData>(file);
            if (data != null) this.data = data;

            return true;
        }

    }
}