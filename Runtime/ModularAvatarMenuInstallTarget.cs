using nadena.dev.modular_avatar.core.menu;
using UnityEngine;

namespace nadena.dev.modular_avatar.core
{
    /// <summary>
    /// The menu install target includes the controls of the target menu installer at the point of reference.
    /// Notably, this can include multiple controls.
    ///
    /// One tricky aspect of this feature is that we need to disambiguate when a menu installer also cites a target menu.
    ///  Generally, if an installer is targeted by any menu install target (even if - especially if - disabled), we
    /// ignore its install target configuration entirely.
    ///
    /// We can also end up with a loop between install targets; in this case, we break the loop at an arbitrary point.
    /// </summary>
#if MA_VRCSDK3_AVATARS
    [AddComponentMenu("Modular Avatar/MA Menu Install Target")]
#else
    [AddComponentMenu("Modular Avatar/Unsupported/MA Menu Install Target (Unsupported)")]
#endif
    internal class ModularAvatarMenuInstallTarget : MenuSourceComponent
    {
        public ModularAvatarMenuInstaller installer;

        public override void Visit(NodeContext context)
        {
            context.PushNode(installer);
        }

        public override void ResolveReferences()
        {
            // no-op
        }
    }
}