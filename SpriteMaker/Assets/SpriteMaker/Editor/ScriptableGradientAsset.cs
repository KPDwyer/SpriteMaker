using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpriteMaker
{
    /// <summary>
    /// we need a serializable object witha  gradient to show a nice gradient editor. see the comment in DrawPerlin.cs
    /// </summary>
    public class ScriptableGradientAsset : ScriptableObject
    {
        public Gradient gradient;
    }
}
