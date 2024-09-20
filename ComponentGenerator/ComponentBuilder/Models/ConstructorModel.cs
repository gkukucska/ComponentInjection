using ComponentGenerator.ComponentBuilder.Models.Parameters;
using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models
{
    internal class ConstructorModel
    {

        public IReadOnlyCollection<ParameterModelBase> Parameters { get;}

        public ConstructorModel(IReadOnlyCollection<ParameterModelBase> parameters)
        {
            Parameters = parameters;
        }
    }
}