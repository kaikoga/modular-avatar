using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

#if MA_VRC
using VRC.SDK3.Avatars.Components;
#endif

namespace nadena.dev.modular_avatar.core
{
    public class AvatarRoot
    {
#if MA_VRC
        public readonly VRCAvatarDescriptor vrcAvatarDescriptor;

        public GameObject gameObject => vrcAvatarDescriptor.gameObject; 
        public Component component => vrcAvatarDescriptor; 
        public Transform transform => vrcAvatarDescriptor.transform; 

        AvatarRoot(VRCAvatarDescriptor vrcAvatarDescriptor)
        {
            this.vrcAvatarDescriptor = vrcAvatarDescriptor;
        }

        [CanBeNull]
        public static AvatarRoot AsAvatarRoot(Component target)
        {
            var av = target.GetComponent<VRCAvatarDescriptor>();
            return av ? new AvatarRoot(av) : null;
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

        public GameObject gameObject => animator.gameObject; 
        public Component component => animator; 
        public Transform transform => animator.transform; 

        AvatarRoot(Animator animator)
        {
            this.animator = animator;
        }

        [CanBeNull]
        public static AvatarRoot AsAvatarRoot(Component target)
        {
            var an = target.GetComponent<Animator>();
            if (!an) return null;
            var parent = target.transform.parent;
            if (parent && parent.GetComponentInParent<Animator>()) return null;
            return new AvatarRoot(an);
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

        public static bool IsAvatarRoot(Component target) => AsAvatarRoot(target) != null;

        public static bool IsAvatarRoot(GameObject gameObject) => AsAvatarRoot(gameObject.transform) != null;

        static IEnumerable<AvatarRoot> GetAvatarRoots<T>(Scene scene)
            where T : Component
        {
            foreach (var root in scene.GetRootGameObjects())
            {
                foreach (var avatar in root.GetComponentsInChildren<T>())
                {
                    var a = AsAvatarRoot(avatar);
                    if (a != null) yield return a;
                }
            }
        }

        public static IEnumerable<AvatarRoot> GetAvatarRoots(Scene scene)
        {
#if MA_VRC
            return GetAvatarRoots<VRCAvatarDescriptor>(scene);
#else
            return GetAvatarRoots<Animator>(scene);
#endif
        }
    }
}