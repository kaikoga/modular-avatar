﻿using UnityEngine;

#if MA_VRC
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace nadena.dev.modular_avatar.core
{
    [AddComponentMenu("Modular Avatar/MA Menu Installer")]
    public class ModularAvatarMenuInstaller : AvatarTagComponent
    {
#if MA_VRC
        public VRCExpressionsMenu menuToAppend;
        public VRCExpressionsMenu installTargetMenu;
#else
        public ScriptableObject menuToAppend;
        public ScriptableObject installTargetMenu;
#endif

        // ReSharper disable once Unity.RedundantEventFunction
        void Start()
        {
            // Ensure that unity generates an enable checkbox
        }

        protected override void OnValidate()
        {
            base.OnValidate();

            RuntimeUtil.InvalidateMenu();
        }
    }
}