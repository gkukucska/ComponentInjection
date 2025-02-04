using ComponentGenerator.Common.Models.Parameters;
using System.Collections.Generic;
using System.Linq;

namespace ComponentGenerator.Common.Models
{
    internal class ConstructorModel
    {

        public IReadOnlyCollection<ParameterModelBase> Parameters { get; }

        public ConstructorModel(IReadOnlyCollection<ParameterModelBase> parameters)
        {
            Parameters = parameters;
        }

        public override bool Equals(object obj)
        {
            return obj is ConstructorModel model &&
                   Enumerable.SequenceEqual(Parameters, model.Parameters);
        }

        public override int GetHashCode()
        {
            return -1807917087 + Parameters.Select(x=>x.GetHashCode()).Sum();
        }

        public static bool operator ==(ConstructorModel left, ConstructorModel right)
        {
            return EqualityComparer<ConstructorModel>.Default.Equals(left, right);
        }

        public static bool operator !=(ConstructorModel left, ConstructorModel right)
        {
            return !(left == right);
        }
    }
}