using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class ServiceModel : InjectableModelBase
    {
        public ServiceModel(string className, List<string> implementationCollection, string lifetime) : base(className, implementationCollection, lifetime)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(ServiceModel left, ServiceModel right)
        {
            return EqualityComparer<ServiceModel>.Default.Equals(left, right);
        }

        public static bool operator !=(ServiceModel left, ServiceModel right)
        {
            return !(left == right);
        }
    }
}