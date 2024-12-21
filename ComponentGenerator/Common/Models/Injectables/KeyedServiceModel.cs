using System.Collections.Generic;
using System.Linq;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class KeyedServiceModel : ServiceModel
    {

        public string ServiceKey { get; }

        public KeyedServiceModel(string className, List<string> implementationCollection, string lifetime, string serviceKey) : base(className, implementationCollection, lifetime)
        {
            ServiceKey = serviceKey;
        }

        public override bool Equals(object obj)
        {
            return obj is KeyedServiceModel model &&
                   base.Equals(obj) &&
                   ServiceKey == model.ServiceKey;
        }

        public override int GetHashCode()
        {
            int hashCode = -978744198;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ServiceKey);
            return hashCode;
        }

        public static bool operator ==(KeyedServiceModel left, KeyedServiceModel right)
        {
            return EqualityComparer<KeyedServiceModel>.Default.Equals(left, right);
        }

        public static bool operator !=(KeyedServiceModel left, KeyedServiceModel right)
        {
            return !(left == right);
        }
    }
}