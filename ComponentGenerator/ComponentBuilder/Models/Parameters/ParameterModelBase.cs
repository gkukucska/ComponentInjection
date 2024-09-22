namespace ComponentGenerator.ComponentBuilder.Models.Parameters
{
    internal abstract class ParameterModelBase
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