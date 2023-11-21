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
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            };
            string jsonStr = JsonConvert.SerializeObject(data, settings);
            File.WriteAllText(path, jsonStr);
        }

        public bool LoadGame() {
            if (!File.Exists(path)) return false;
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,
                PreserveReferencesHandling = PreserveReferencesHandling.All,
            };
            string file = File.ReadAllText(path);
            GameData data = JsonConvert.DeserializeObject<GameData>(file, settings);
            if (data != null) this.data = data;
            //if (string.IsNullOrEmpty(data.character.Name)) return false; //이 부분이 있으면 게임을 키고 그냥 껐을때 다시 실행이안됌.

            return true;
        }

    }
}