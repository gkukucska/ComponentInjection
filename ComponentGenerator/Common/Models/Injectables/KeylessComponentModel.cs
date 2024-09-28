using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class KeylessComponentModel : ComponentModel
    {
        public KeylessComponentModel(string className, ConstructorModel constructor, List<string> implementationCollection, string lifetime, string optionType) : base(className, constructor, implementationCollection, lifetime, optionType)
        {
        }
    }
}