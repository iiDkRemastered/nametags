using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NameTags.Managers
{
    internal class Friends : MonoBehaviour
    {
        public static Dictionary<string, string> Names = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private const string URL = "https://gtag.website/data/index.txt";

        void Awake()
        {
            StartCoroutine(FetchRoutine());
        }

        IEnumerator FetchRoutine()
        {
            while (true)
            {
                string url = URL + "?t=" + DateTime.UtcNow.Ticks;
                yield return StartCoroutine(Fetch(url));
                yield return new WaitForSeconds(30f);
            }
        }

        IEnumerator Fetch(string url)
        {
            using (var req = UnityEngine.Networking.UnityWebRequest.Get(url))
            {
                yield return req.SendWebRequest();

                if (req.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError ||
                    req.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("[NameTags] Fetch error: " + req.error);
                    yield break;
                }

                Parse(req.downloadHandler.text);
            }
        }

        void Parse(string data)
        {
            try
            {
                Names.Clear();
                string[] lines = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in lines)
                {
                    int sep = line.IndexOf(':');
                    if (sep <= 0) continue;
                    string name = line.Substring(0, sep).Trim();
                    string id = line.Substring(sep + 1).Trim();
                    if (name.Length > 0 && id.Length > 0)
                        Names[id] = name;
                }
                Debug.Log("[NameTags] Loaded " + Names.Count + " friends");
            }
            catch (Exception e)
            {
                Debug.LogError("[NameTags] Parse error: " + e.Message);
            }
        }
    }
}
