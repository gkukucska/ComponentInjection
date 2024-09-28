using ComponentGenerator.Common.Models.Parameters;
using System.Collections.Generic;

namespace ComponentGenerator.Common.Models
{
    internal class ConstructorModel
    {

        public IReadOnlyCollection<ParameterModelBase> Parameters { get; }

        public ConstructorModel(IReadOnlyCollection<ParameterModelBase> parameters)
        {
            Parameters = parameters;
        }
    }
}