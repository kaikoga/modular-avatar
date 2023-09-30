using nadena.dev.modular_avatar.core.menu;
using UnityEngine;

#if MA_VRCSDK3_AVATARS
using VRC.SDK3.Avatars.ScriptableObjects;
#endif

namespace nadena.dev.modular_avatar.core
{
    public enum SubmenuSource
    {
        MenuAsset,
        Children,
    }

    [AddComponentMenu("Modular Avatar/MA Menu Item")]
    public class ModularAvatarMenuItem : AvatarTagComponent, MenuSource
    {
#if MA_VRCSDK3_AVATARS
        public VRCExpressionsMenu.Control Control;
#endif
        public SubmenuSource MenuSource;

        public GameObject menuSource_otherObjectChildren;

        /// <summary>
        /// If no control group is set (and an action is linked), this controls whether this control is synced.
        /// </summary>
        public bool isSynced = true;

        public bool isSaved = true;

#if MA_VRCSDK3_AVATARS

        protected override void OnValidate()
        {
            base.OnValidate();

            RuntimeUtil.InvalidateMenu();

            if (Control == null)
            {
                Control = new VRCExpressionsMenu.Control();
            }
        }

        public override void ResolveReferences()
        {
            // no-op
        }
#endif

        public void Visit(NodeContext context)
        {
#if MA_VRCSDK3_AVATARS
            if (Control == null)
            {
                Control = new VRCExpressionsMenu.Control();
            }

            var cloned = new VirtualControl(Control);
            cloned.subMenu = null;
            cloned.name = gameObject.name;

            if (cloned.type == VRCExpressionsMenu.Control.ControlType.SubMenu)
            {
                switch (this.MenuSource)
                {
                    case SubmenuSource.MenuAsset:
                        cloned.SubmenuNode = context.NodeFor(this.Control.subMenu);
                        break;
                    case SubmenuSource.Children:
                    {
                        var root = this.menuSource_otherObjectChildren != null
                            ? this.menuSource_otherObjectChildren
                            : this.gameObject;

                        cloned.SubmenuNode = context.NodeFor(new MenuNodesUnder(root));
                        break;
                    }
                }
            }

            context.PushControl(cloned);
#endif
        }
    }
}

