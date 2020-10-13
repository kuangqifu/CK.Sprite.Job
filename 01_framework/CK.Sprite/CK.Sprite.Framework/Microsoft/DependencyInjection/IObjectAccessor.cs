using JetBrains.Annotations;

namespace CK.Sprite.Framework
{
    public interface IObjectAccessor<out T>
    {
        [CanBeNull]
        T Value { get; }
    }
}
