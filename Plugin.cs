using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace NameTags
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private List<VRRig> ntGiven = new List<VRRig> { };
        void Update()
        {
            if (GorillaLocomotion.GTPlayer.Instance == null)
                return;

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != GorillaTagger.Instance.offlineVRRig && !ntGiven.Contains(vrrig))
                {
                    ntGiven.Add(vrrig);

                    GameObject NameTagObject = LoadAsset("NameTag");
                    NameTagObject.transform.SetParent(vrrig.headMesh.transform, false);

                    GameObject Canvas = NameTagObject.transform.Find("Canvas").gameObject;

                    NameTag a = Canvas.AddComponent<NameTag>();
                    a.text = Canvas.transform.Find("Name").GetComponent<UnityEngine.UI.Text>();
                    a.rig = vrrig;
                }
            }
        }

        static AssetBundle assetBundle;
        public static GameObject LoadAsset(string assetName)
        {
            GameObject gameObject = null;

            if (assetBundle == null)
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NameTags.Resources.nametag");
                assetBundle = AssetBundle.LoadFromStream(stream);
                stream.Close();
            }
            gameObject = Instantiate(assetBundle.LoadAsset<GameObject>(assetName));

            return gameObject;
        }
    }
}
