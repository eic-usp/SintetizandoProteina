using System;
using System.IO;
using UnityEngine;

using Field = Game.Scripts.Gameplay.Generic.GenericScriptableObjectToJson.Field;

namespace Game.Scripts.Gameplay.Generic
{
    public static class GenericScriptableObjectToJsonParser
    {
        public static string GetJson(GenericScriptableObjectToJson scriptable)
        {
            string json = "{";

            for (int i = 0; i < scriptable.fields.Length; i++)
            {
                json += RecursionWriteField(scriptable.fields[i], true, i == scriptable.fields.Length - 1 ? "" : ",");
            }

            json += "}";
            
            return json;
        }

        private static string RecursionWriteField(Field field, bool checkName, string ending)
        {
            var jsonField = "";
            var additionalEnding = "";
            var validName = field.ValidNameField;
            bool isClass = field.IsClass;
            
            if (checkName && !validName)
            {
                throw new InvalidDataException();
            }
            
            //Add name
            if (validName) jsonField += $"\"{field.nameField}\":";
            
            //Add content
            if (field.ValidContent) jsonField += GetContent(field);
            
            //Add special marks
            jsonField += GetMarks(field, isClass, ref additionalEnding);
            
            //Loop to get all the data
            for (int i = 0; i < field.extraData.Length; i++)
            {
                jsonField += RecursionWriteField(field.extraData[i], 
                                                    isClass,
                                                    i == field.extraData.Length - 1 ? "" : ",");
            }

            jsonField += $"{additionalEnding}{ending}";
            
            return jsonField;
        }

        public static void CreateFile(GenericScriptableObjectToJson scriptable, string savePath)
        {
            // Get the path
            var path = Application.dataPath + "/" + savePath + "/" + scriptable.nameFile + ".json";
            
            try
            {
                // Write the string to the file
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    // Get the JSON content from the ScriptableObject
                    string jsonContent = GetJson(scriptable);

                    // Write the JSON content to the file
                    writer.WriteLine(jsonContent);
                    writer.Close();
                }
                Debug.Log("File created successfully at: " + path);
            }
            catch (Exception ex)
            {
                Debug.LogError("An error occurred: " + ex.Message);
            }
        }
        
        private static string GetContent(Field field)
        {
            if (field.typeContent == GenericScriptableObjectToJson.FieldType.TypeString)
            {
                return $"\"{field.content}\"";
            }

            return field.content;
        }

        private static string GetMarks(Field field, bool isClass, ref string ending)
        {
            if (isClass)
            {
                ending = "}";
                return "{";
            }
            
            if (field.IsVector)
            {
                ending = "]";
                return "[";
            }

            return "";
        }
    }
}
