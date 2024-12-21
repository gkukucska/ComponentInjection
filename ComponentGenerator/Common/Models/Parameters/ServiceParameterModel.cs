using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Parameters
{
    internal class ServiceParameterModel : ParameterModelBase
    {
        public ServiceParameterModel(string name, string type, bool isOptional) : base(name, type, isOptional)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is ServiceParameterModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }

        public static bool operator ==(ServiceParameterModel left, ServiceParameterModel right)
        {
            return EqualityComparer<ServiceParameterModel>.Default.Equals(left, right);
        }

        public static bool operator !=(ServiceParameterModel left, ServiceParameterModel right)
        {
            return !(left == right);
        }
    }
}