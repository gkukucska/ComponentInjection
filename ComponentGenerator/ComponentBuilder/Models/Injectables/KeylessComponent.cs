using System.Collections.Generic;

namespace ComponentGenerator.ComponentBuilder.Models.Injectables
{
    internal class KeylessComponent : ComponentModel
    {
        public KeylessComponent(string nameSpace, string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, constructor, implementationCollection, lifetime, optionType)
        {
        }
    }
}