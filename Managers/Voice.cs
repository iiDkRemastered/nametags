using Photon.Pun;
using UnityEngine;
using UnityEngine.Networking;

namespace NameTags.Managers
{
    internal class Voice : MonoBehaviour
    {
        public static Texture2D SpeakerIcon;

        void Start()
        {
            StartCoroutine(LoadIcon());
        }

        System.Collections.IEnumerator LoadIcon()
        {
            using (var req = UnityWebRequestTexture.GetTexture("https://iili.io/nRFy3TQ.png"))
            {
                yield return req.SendWebRequest();
                if (req.result == UnityWebRequest.Result.Success)
                {
                    SpeakerIcon = DownloadHandlerTexture.GetContent(req);
                }
            }
        }

        public static bool IsSpeaking(VRRig rig)
        {
            if (rig == null) return false;
            
            var loudness = rig.GetComponent<GorillaSpeakerLoudness>();
            return loudness != null && loudness.IsSpeaking;
        }
    }
}
