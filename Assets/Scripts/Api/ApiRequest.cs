using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using Newtonsoft.Json;
using UI.GridLayout;
using UnityEngine;
using UnityEngine.Networking;

namespace Api
{
    public class ApiRequest : MonoBehaviour
    {
        private const string Uri = "https://speedjamleaderboardapi.azurewebsites.net/Leaderboard";

        private List<LeaderboardEntry> _leaderboardEntries = new List<LeaderboardEntry>();

        public void StartGetRequest()
        {
            StartCoroutine(GetRequest(Uri));
        }
    
        public void StartGetItemRequest(string id)
        {
            StartCoroutine(GetItemRequest(Uri + "/" + id));
        }
    
        public void StartPostRequest(string itemName, string score)
        {
            StartCoroutine(PostRequest(Uri, itemName, score));
        }
    
        public void StartPutRequest(string id, string itemName, string score)
        {
            StartCoroutine(PutRequest(Uri + "/" + id, itemName, score));
        }
    
        public void StartDeleteRequest(string id)
        {
            StartCoroutine(DeleteRequest(Uri + "/" + id));
        }

        private IEnumerator GetRequest(string uri)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    JsonStringToObjectList(webRequest.downloadHandler.text);
                    break;
            }
        
            webRequest.Dispose();
        }
        
        private IEnumerator GetItemRequest(string uri)
        {
            using UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    JsonStringToObject(webRequest.downloadHandler.text);
                    break;
            }
        
            webRequest.Dispose();
        }

        private IEnumerator PostRequest(string uri, string itemName, string score)
        {
            string json = "{\"name\":\"" + itemName + "\",\"score\":\"" + score + "\"}";

            UnityWebRequest webRequest = new UnityWebRequest(uri, "POST");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            } else
            {
                Debug.Log("Form upload complete!");
                // Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        
            webRequest.Dispose();
        }

        private IEnumerator PutRequest(string uri, string itemName, string score)
        {
            string json = "{\"name\":\"" + itemName + "\",\"score\":\"" + score + "\"}";

            UnityWebRequest webRequest = new UnityWebRequest(uri, "PUT");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            } else
            {
                Debug.Log("Form update complete!");
                // Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        
            webRequest.Dispose();
        }

        private IEnumerator DeleteRequest(string uri)
        {
            UnityWebRequest webRequest = new UnityWebRequest(uri, "DELETE");
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.SetRequestHeader("Accept", "application/json");
            webRequest.downloadHandler = new DownloadHandlerBuffer();
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
        
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
            } else
            {
                Debug.Log("Form delete complete!");
                // Debug.Log(":\nReceived: " + webRequest.downloadHandler.text);
            }
        }

        private void JsonStringToObjectList(string jsonString)
        {
            _leaderboardEntries = JsonConvert.DeserializeObject<List<LeaderboardEntry>>(jsonString);
        
            foreach (var entry in _leaderboardEntries)
            {
                Debug.Log(entry.Id + " " + entry.Name + " " + entry.Score);
            }
            
            // Sort the list by score parsing it to int
            try
            {
                _leaderboardEntries.Sort((x, y) => int.Parse(y.Score).CompareTo(int.Parse(x.Score)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            FindObjectOfType<GridController>().SetGridCells(_leaderboardEntries);
        }
        
        private void JsonStringToObject(string jsonString)
        {
            var leaderboardEntry = JsonConvert.DeserializeObject<LeaderboardEntry>(jsonString);
            
            FindObjectOfType<GridController>().SetGridCells(new List<LeaderboardEntry> {leaderboardEntry});
        }
    }
}