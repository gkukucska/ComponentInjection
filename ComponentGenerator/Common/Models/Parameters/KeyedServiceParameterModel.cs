using Microsoft.CodeAnalysis;

namespace ComponentGenerator.Common.Models.Parameters
{
    internal class KeyedServiceParameterModel : ParameterModelBase
    {
        public string ServiceKey { get; }

        public KeyedServiceParameterModel(string name, string type, string serviceKey, bool isOptional) : base(name, type, isOptional)
        {
            ServiceKey = serviceKey;
        }
    }
}