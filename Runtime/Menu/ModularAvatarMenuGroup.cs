using nadena.dev.modular_avatar.core.menu;
using UnityEngine;

namespace nadena.dev.modular_avatar.core
{
#if MA_VRCSDK3_AVATARS
    [AddComponentMenu("Modular Avatar/MA Menu Group")]
#else
    [AddComponentMenu("Modular Avatar/Unsupported/MA Menu Group (Unsupported)")]
#endif
    public class ModularAvatarMenuGroup : MenuSourceComponent
    {
        public GameObject targetObject;

        public override void Visit(NodeContext context)
        {
#if MA_VRCSDK3_AVATARS

            context.PushNode(new MenuNodesUnder(targetObject != null ? targetObject : gameObject));
#endif
        }

        public override void ResolveReferences()
        {
            // no-op
        }
    }
}

