﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace nadena.dev.modular_avatar.core
{
    [Serializable]
    public struct ParameterConfig
    {
        public string nameOrPrefix;
        public string remapTo;
        public bool internalParameter, isPrefix;
        public ParameterSyncType syncType;
        public bool localOnly;
        public float defaultValue;
        public bool saved;
    }

    /**
     * This enum is a bit poorly named, having been introduced before local-only parameters were a thing. In actuality,
     * this is the parameter type - NotSynced indicates the parameter should not be registered in Expression Parameters.
     */
    public enum ParameterSyncType
    {
        NotSynced,
        Int,
        Float,
        Bool,
    }

    [DisallowMultipleComponent]
#if MA_VRCSDK3_AVATARS
    [AddComponentMenu("Modular Avatar/MA Parameters")]
#else
    [AddComponentMenu("Modular Avatar/Unsupported/MA Parameters (Unsupported)")]
#endif
    public class ModularAvatarParameters : AvatarTagComponent
    {
        public List<ParameterConfig> parameters = new List<ParameterConfig>();

        public override void ResolveReferences()
        {
            // no-op
        }
    }
}