using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class KeylessComponentModel : ComponentModel
    {
        public KeylessComponentModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, constructor, implementationCollection, lifetime, optionType)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is KeylessComponentModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(KeylessComponentModel left, KeylessComponentModel right)
        {
            return EqualityComparer<KeylessComponentModel>.Default.Equals(left, right);
        }

        public static bool operator !=(KeylessComponentModel left, KeylessComponentModel right)
        {
            return !(left == right);
        }
    }
}