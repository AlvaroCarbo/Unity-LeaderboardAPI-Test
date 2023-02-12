using System;
using Enums;
using TMPro;
using UnityEngine.Serialization;

namespace Model
{
    [Serializable]
    public class MyInputField
    {
        public InputFieldType type;
        public TMP_InputField input;
        public string value;
        
        public MyInputField(InputFieldType type, TMP_InputField input)
        {
            this.type = type;
            this.input = input;
        }
        
        public void SetValue(string newValue)
        {
            value = newValue;
        }
    }
}