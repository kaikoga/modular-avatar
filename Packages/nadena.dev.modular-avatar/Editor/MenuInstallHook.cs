#if MA_VRC

using System;
using System.Collections.Generic;
using System.Linq;
using nadena.dev.modular_avatar.core.editor.menu;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;
using VRC.SDK3.Avatars.ScriptableObjects;


namespace nadena.dev.modular_avatar.core.editor
{
    internal class MenuInstallHook
    {
        private static Texture2D _moreIcon = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Packages/nadena.dev.modular-avatar/Runtime/Icons/Icon_More_A.png"
        );

        private BuildContext _context;

        private VRCExpressionsMenu _rootMenu;

        private Stack<ModularAvatarMenuInstaller> _visitedInstallerStack;

        public void OnPreprocessAvatar(GameObject avatarRoot, BuildContext context)
        {
            _context = context;

            ModularAvatarMenuInstaller[] menuInstallers = avatarRoot
                .GetComponentsInChildren<ModularAvatarMenuInstaller>(true)
                .Where(menuInstaller => menuInstaller.enabled)
                .ToArray();
            if (menuInstallers.Length == 0) return;

            _visitedInstallerStack = new Stack<ModularAvatarMenuInstaller>();

            AvatarRoot avatar = AvatarRoot.AsAvatarRoot(avatarRoot);

            if (avatar.vrcAvatarDescriptor.expressionsMenu == null)
            {
                var menu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                _context.SaveAsset(menu);
                avatar.vrcAvatarDescriptor.expressionsMenu = menu;
                context.ClonedMenus[menu] = menu;
            }

            _rootMenu = avatar.vrcAvatarDescriptor.expressionsMenu;
            var virtualMenu = VirtualMenu.ForAvatar(avatar, context);
            avatar.vrcAvatarDescriptor.expressionsMenu = virtualMenu.SerializeMenu(asset =>
            {
                context.SaveAsset(asset);
                if (asset is VRCExpressionsMenu menu) SplitMenu(menu);
            });
        }

        private void SplitMenu(VRCExpressionsMenu targetMenu)
        {
            while (targetMenu.controls.Count > VRCExpressionsMenu.MAX_CONTROLS)
            {
                // Split target menu
                var newMenu = ScriptableObject.CreateInstance<VRCExpressionsMenu>();
                _context.SaveAsset(newMenu);
                const int keepCount = VRCExpressionsMenu.MAX_CONTROLS - 1;
                newMenu.controls.AddRange(targetMenu.controls.Skip(keepCount));
                targetMenu.controls.RemoveRange(keepCount,
                    targetMenu.controls.Count - keepCount
                );

                targetMenu.controls.Add(new VRCExpressionsMenu.Control
                {
                    name = "More",
                    type = VRCExpressionsMenu.Control.ControlType.SubMenu,
                    subMenu = newMenu,
                    parameter = new VRCExpressionsMenu.Control.Parameter
                    {
                        name = ""
                    },
                    subParameters = Array.Empty<VRCExpressionsMenu.Control.Parameter>(),
                    icon = _moreIcon,
                    labels = Array.Empty<VRCExpressionsMenu.Control.Label>()
                });

                targetMenu = newMenu;
            }
        }
    }
}

#endif