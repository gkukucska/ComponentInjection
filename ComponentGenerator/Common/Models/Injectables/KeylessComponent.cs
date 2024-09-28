using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class KeylessComponent : ComponentModel
    {
        public ConstructorModel Constructor { get; }

        public KeylessComponent(string nameSpace, string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, constructor, implementationCollection, lifetime, optionType)
        {
            Constructor = constructor;
        }
    }
}