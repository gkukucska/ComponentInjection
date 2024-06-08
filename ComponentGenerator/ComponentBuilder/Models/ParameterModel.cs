namespace ComponentGenerator.ComponentBuilder.Models
{
    internal class ParameterModel
    {
        public ParameterModel()
        {
        }

        public string Name { get; internal set; }
        public string Type { get; internal set; }
        public bool IsAlias { get; internal set; }
    }
}