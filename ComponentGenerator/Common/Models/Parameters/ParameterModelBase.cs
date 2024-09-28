namespace ComponentGenerator.Common.Models.Parameters
{
    internal abstract class ParameterModelBase
    {
        public string Name { get; }

        public string Type { get; }

        public bool IsOptional { get; }

        public ParameterModelBase(string name, string type,bool isOptional)
        {
            Name = name;
            Type = type;
            IsOptional = isOptional;
        }
    }
}