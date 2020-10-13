using System;

namespace CK.Sprite.Framework
{
    public interface IExposedServiceTypesProvider
    {
        Type[] GetExposedServiceTypes(Type targetType);
    }
}
