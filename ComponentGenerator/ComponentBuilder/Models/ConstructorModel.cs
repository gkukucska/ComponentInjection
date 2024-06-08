using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models
{
    internal class ConstructorModel
    {
        public ConstructorModel()
        {
        }

        public List<ParameterModel> Parameters { get; internal set; } = new List<ParameterModel>();
    }
}