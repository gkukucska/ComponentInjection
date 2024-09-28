using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{

    internal class ComponentModel : InjectableModelBase
    {
        public string OptionType { get; }

        public ConstructorModel Constructor { get; }

        public ComponentModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, implementationCollection, lifetime)
        {
            OptionType = optionType;
            Constructor = constructor;
        }
    }
}