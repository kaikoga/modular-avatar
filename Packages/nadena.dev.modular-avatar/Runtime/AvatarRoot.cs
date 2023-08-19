using UnityEngine;

#if MA_VRC
using VRC.SDK3.Avatars.Components;
#endif

namespace nadena.dev.modular_avatar.core
{
    public class AvatarRoot
    {
#if MA_VRC
        public readonly VRCAvatarDescriptor vrcAvatarDescriptor;

        AvatarRoot(VRCAvatarDescriptor vrcAvatarDescriptor)
        {
            this.vrcAvatarDescriptor = vrcAvatarDescriptor;
        }

        public static AvatarRoot FindAvatarInParents(Transform target)
        {
            while (target != null)
            {
                var av = target.GetComponent<VRCAvatarDescriptor>();
                if (av != null) return av;
                target = target.parent;
            }

            return null;
        }
#else
        public readonly Animator animator;

        AvatarRoot(Animator animator)
        {
            this.animator = animator;
        }

        public static AvatarRoot FindAvatarInParents(Transform target)
        {
            Animator av = null; 
            while (target != null)
            {
                var an = target.GetComponent<Animator>();
                if (an != null) av = an;
                target = target.parent;
            }
            return av ? new AvatarRoot(av) : null;

        }
#endif
        
    }
}