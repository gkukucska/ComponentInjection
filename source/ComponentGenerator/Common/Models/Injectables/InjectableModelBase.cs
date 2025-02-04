using System.Collections.Generic;
using System.Linq;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class InjectableModelBase
    {
        public string ClassName { get; }
        public List<string> ImplementationCollection { get; }
        public string Lifetime { get; }

        public InjectableModelBase(string className, List<string> implementationCollection, string lifetime)
        {
            ClassName = className;
            ImplementationCollection = implementationCollection;
            Lifetime = lifetime;
        }

        public override bool Equals(object obj)
        {
            return obj is InjectableModelBase @base &&
                   ClassName == @base.ClassName &&
                   Enumerable.SequenceEqual(ImplementationCollection, @base.ImplementationCollection) &&
                   Lifetime == @base.Lifetime;
        }

        public override int GetHashCode()
        {
            int hashCode = 1347511593;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ClassName);
            foreach (var implementation in ImplementationCollection)
            {
                hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(implementation);
            }
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Lifetime);
            return hashCode;
        }

        public static bool operator ==(InjectableModelBase left, InjectableModelBase right)
        {
            return EqualityComparer<InjectableModelBase>.Default.Equals(left, right);
        }

        public static bool operator !=(InjectableModelBase left, InjectableModelBase right)
        {
            return !(left == right);
        }
    }
}