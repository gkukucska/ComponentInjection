using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class ServiceModel : InjectableModelBase
    {
        public ServiceModel(string className, List<string> implementationCollection, string lifetime) : base(className, implementationCollection, lifetime)
        {
        }
    }
}