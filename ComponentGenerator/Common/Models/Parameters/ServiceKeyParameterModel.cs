using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Parameters
{
    internal class ServiceKeyParameterModel : ParameterModelBase
    {
        public ServiceKeyParameterModel(string name, string type, bool isOptional) : base(name, type, isOptional)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceKeyParameterModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(ServiceKeyParameterModel left, ServiceKeyParameterModel right)
        {
            return EqualityComparer<ServiceKeyParameterModel>.Default.Equals(left, right);
        }

        public static bool operator !=(ServiceKeyParameterModel left, ServiceKeyParameterModel right)
        {
            return !(left == right);
        }
    }
}