using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace NameTags
{
    internal class NameTag : MonoBehaviour
    {
        public UnityEngine.UI.Text text;
        public VRRig rig;

        void LateUpdate()
        {
            if (rig.enabled)
            {
                transform.LookAt(GorillaTagger.Instance.headCollider.transform.position);
                text.text = rig.Creator.NickName;
                text.color = rig.mainSkin.material.name.Contains("fected") ? new Color(1f, 0.5f, 0f, 1) : rig.playerColor;
            } else
                text.text = "null";
        }

        // The-Graze/WhoIsTalking
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
