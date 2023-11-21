using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject {

    [Serializable]
    public class GameData {
        public Character character;
    }
    public class GameManager {

        public GameData data = new();
        private string path;

        public void Initialize() {
            path = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "/SaveData.json";
            if (LoadGame()) return;
        }

        public void SaveGame() {
            var settings = new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
            };
            string jsonStr = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(path, jsonStr);
        }

        public bool LoadGame() {
            if (!File.Exists(path)) return false;

            string file = File.ReadAllText(path);
            GameData data = JsonConvert.DeserializeObject<GameData>(file);
            if (data != null) this.data = data;
            if (string.IsNullOrEmpty(data.character.Name)) return false;

            return true;
        }

    }
}