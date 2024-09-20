namespace ComponentGenerator.ComponentBuilder.Models.Parameters
{
    internal class ParameterModelBase
    {
        public string Name { get; }

        public string Type { get; }

        public ParameterModelBase(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}