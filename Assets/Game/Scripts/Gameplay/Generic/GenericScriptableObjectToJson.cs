using System;
using UnityEngine;

namespace Game.Scripts.Gameplay.Generic
{
    [CreateAssetMenu(fileName = "Data", menuName = "GenericScriptableObjectToJson")]
    public class GenericScriptableObjectToJson : ScriptableObject
    {
        public string nameFile;

        [Serializable]
        public enum FieldType
        {
            TypeString,
            TypeNumerical,
        }
        
        [Serializable]
        public class Field
        {
            public string nameField;
            public FieldType typeContent;
            public string content;
            public Field[] extraData;

            public bool ValidContent => !string.IsNullOrEmpty(content) && !string.IsNullOrWhiteSpace(content);
            public bool ValidNameField => !string.IsNullOrEmpty(nameField) && !string.IsNullOrWhiteSpace(nameField);

            public bool IsClass => extraData.Length > 0 && extraData[0].ValidNameField;
            public bool IsVector => extraData.Length > 0;
        }

        public Field[] fields;
        
    }
}
