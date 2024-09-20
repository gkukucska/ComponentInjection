namespace ComponentGenerator.ComponentBuilder.Models.Parameters
{
    internal class KeyedServiceParameterModel : ParameterModelBase
    {
        public string ServiceKey { get; }

        public KeyedServiceParameterModel(string name, string type, string serviceKey) : base(name, type)
        {
            ServiceKey = serviceKey;
        }
    }
}