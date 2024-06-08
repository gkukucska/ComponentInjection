namespace ComponentGenerator.ComponentBuilder.Models
{
    internal class ComponentModel
    {
        public string Namespace { get; internal set; }
        public string ClassName { get; internal set; }
        public ConstructorModel Constructor { get; internal set; }
        public string InterfaceType { get; internal set; }
        public string OptionType { get; internal set; }
        public string Lifetime { get; internal set; }
    }
}