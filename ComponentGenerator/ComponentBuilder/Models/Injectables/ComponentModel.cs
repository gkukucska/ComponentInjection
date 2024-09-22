using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models.Injectables
{

    internal class ComponentModel : InjectableModelBase
    {
        public string OptionType { get; }

        public ComponentModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, constructor, implementationCollection, lifetime)
        {
            OptionType = optionType;
        }
    }
}