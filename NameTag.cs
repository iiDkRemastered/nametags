using Photon.Pun;
using UnityEngine;

namespace NameTags
{
    internal class NameTag : MonoBehaviour
    {
        public UnityEngine.UI.Text text;
        public VRRig rig;
        public UnityEngine.UI.RawImage speakerImage;

        void Start()
        {
            if (speakerImage == null)
            {
                GameObject imgObj = new GameObject("SpeakerIcon");
                imgObj.transform.SetParent(transform, false);
                speakerImage = imgObj.AddComponent<UnityEngine.UI.RawImage>();
                speakerImage.rectTransform.sizeDelta = new Vector2(5, 5); 
                speakerImage.rectTransform.anchoredPosition = new Vector2(0, 7);
                speakerImage.rectTransform.localScale = new Vector3(-1, 1, 1);
                speakerImage.enabled = false;
            }
        }

        void LateUpdate()
        {
            if (rig != null && rig.enabled && rig.Creator != null)
            {
                transform.LookAt(GorillaTagger.Instance.mainCamera.transform.position);

                string userId = rig.Creator.UserId;
                string nickName = rig.Creator.NickName;

                if (!string.IsNullOrEmpty(userId) && Managers.Friends.Names.ContainsKey(userId))
                    text.text = Managers.Friends.Names[userId];
                else
                    text.text = nickName;

                bool isSpeaking = Managers.Voice.IsSpeaking(rig);
                
                if (speakerImage != null)
                {
                    if (Managers.Voice.SpeakerIcon != null && speakerImage.texture == null)
                        speakerImage.texture = Managers.Voice.SpeakerIcon;
                        
                    speakerImage.enabled = isSpeaking && speakerImage.texture != null;
                }

                text.color = rig.mainSkin.material.name.Contains("fected") ? new Color(1f, 0.5f, 0f, 1) : rig.playerColor;
            } else if (text != null)
            {
                text.text = "null";
                if (speakerImage != null) speakerImage.enabled = false;
            }
        }

        private Color ColourHandling()
        {
            if (rig.bodyRenderer.cosmeticBodyType == GorillaBodyType.Skeleton)
                return Color.green;
            
            switch (rig.setMatIndex)
            {
                case 1:
                    return Color.red;
                case 2:
                case 11:
                    return new Color32(255, 128, 0, 255);
                case 3:
                case 7:
                    return Color.blue;
                case 12:
                    return Color.green;
                default:
                    return rig.playerColor;
            }
        }
    }
}
