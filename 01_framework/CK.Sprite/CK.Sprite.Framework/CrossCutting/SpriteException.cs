using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CK.Sprite.Framework
{
    public class SpriteException : Exception
    {
        public SpriteException(string message) : base(message) { }
    }
}
