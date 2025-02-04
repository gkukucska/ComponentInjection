using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Parameters
{
    internal class KeyedServiceParameterModel : ParameterModelBase
    {
        public string ServiceKey { get; }

        public KeyedServiceParameterModel(string name, string type, string serviceKey, bool isOptional) : base(name, type, isOptional)
        {
            ServiceKey = serviceKey;
        }

        public override bool Equals(object obj)
        {
            return obj is KeyedServiceParameterModel model &&
                   base.Equals(obj) &&
                   ServiceKey == model.ServiceKey;
        }

        public override int GetHashCode()
        {
            int hashCode = -855112935;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ServiceKey);
            return hashCode;
        }
    }
}