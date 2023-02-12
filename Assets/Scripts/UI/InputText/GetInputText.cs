using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using Enums;
using Model;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.InputText
{
    public class GetInputText : MonoBehaviour
    {
        [SerializeField] private List<MyInputField> inputFields = new List<MyInputField>();

        [SerializeField] private Button button;

        [SerializeField] private RequestOperation requestOperation;

        private void Awake()
        {
            foreach (var input in GetComponentsInChildren<TMP_InputField>())
                inputFields.Add(new MyInputField(
                    (InputFieldType) Enum.Parse(typeof(InputFieldType), input.gameObject.name.Split(" ")[0], true),
                    input));

            button = GetComponentInChildren<Button>();

            requestOperation =
                (RequestOperation) Enum.Parse(typeof(RequestOperation), gameObject.name.Replace(" ", ""), true);
        }

        private void Start()
        {
            foreach (var inputField in inputFields) inputField.input.onEndEdit.AddListener(inputField.SetValue);

            button.onClick.AddListener(StartRequest);
        }

        private void StartRequest()
        {
            var api = FindObjectOfType<ApiRequest>();

            var id = GetInputFieldValue(InputFieldType.ID);
            var itemName = GetInputFieldValue(InputFieldType.Name);
            var score = GetInputFieldValue(InputFieldType.Score);

            switch (requestOperation)
            {
                case RequestOperation.Get:
                    api.StartGetRequest();
                    break;
                case RequestOperation.GetItem:
                    api.StartGetItemRequest(id);
                    break;
                case RequestOperation.Post:
                    api.StartPostRequest(itemName, score);
                    break;
                case RequestOperation.Put:
                    api.StartPutRequest(id, itemName, score);
                    break;
                case RequestOperation.Delete:
                    api.StartDeleteRequest(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            ClearInputFields();
        }

        private string GetInputFieldValue(InputFieldType type)
        {
            try
            {
                return inputFields.First(x => x.type == type).value;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return null;
            }
        }

        private void ClearInputFields()
        {
            foreach (var inputField in inputFields)
            {
                inputField.input.text = "";
                inputField.value = "";
            }
        }
    }
}