using System;

namespace Gameplay.Items.Properties
{
    public interface IProperty: IEquatable<IProperty>
    {
        public bool EqualsType(IProperty other);

        public bool EqualsValue(IProperty other);
    }
}
