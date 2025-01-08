using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Injectables
{
    internal class HostedServiceModel
    {
        public string ClassName { get; }
        public ConstructorModel Constructor { get; }
        public string OptionType { get; }

        public HostedServiceModel(string className, ConstructorModel constructor, string optionType)
        {
            ClassName = className;
            Constructor = constructor;
            OptionType = optionType;
        }

        public override bool Equals(object obj)
        {
            return obj is HostedServiceModel model &&
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

        public static bool operator ==(HostedServiceModel left, HostedServiceModel right)
        {
            return EqualityComparer<HostedServiceModel>.Default.Equals(left, right);
        }

        public static bool operator !=(HostedServiceModel left, HostedServiceModel right)
        {
            return !(left == right);
        }
    }
}