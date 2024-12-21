using System;
using System.Collections.Generic;
using System.Linq;

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

        public override bool Equals(object obj)
        {
            return obj is ComponentModel model &&
                   OptionType == model.OptionType &&
                   EqualityComparer<ConstructorModel>.Default.Equals(Constructor, model.Constructor);
        }

        public override int GetHashCode()
        {
            int hashCode = 1985259652;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(OptionType);
            hashCode = hashCode * -1521134295 + EqualityComparer<ConstructorModel>.Default.GetHashCode(Constructor);
            return hashCode;
        }

        public static bool operator ==(ComponentModel left, ComponentModel right)
        {
            return EqualityComparer<ComponentModel>.Default.Equals(left, right);
        }

        public static bool operator !=(ComponentModel left, ComponentModel right)
        {
            return !(left == right);
        }
    }
}