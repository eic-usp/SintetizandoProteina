using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.Generic
{
    public class GenerateAllJson : MonoBehaviour
    {
        public GenericScriptableObjectToJson[] transformToJson;
        public string path = @"Game\Scripts\Gameplay\Phases\Destination\Resources";
        // Start is called before the first frame update
        private void Awake()
        {
            foreach (var so in transformToJson)
            {
                GenericScriptableObjectToJsonParser.CreateFile(so, path);
            }
        }
    }
}
