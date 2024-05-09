using UnityEngine;

namespace Game.Scripts.Gameplay.Generic
{
    public class TestGenericJson : MonoBehaviour
    {
        public GenericScriptableObjectToJson so; 
        public string test;
        void Start()
        {
            GenericScriptableObjectToJsonParser.CreateFile(so, @"Game\Scripts\Gameplay\Phases\Destination\Resources");
        }
    
    }
}
