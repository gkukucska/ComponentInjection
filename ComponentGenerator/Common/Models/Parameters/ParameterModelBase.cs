using System.Collections.Generic;

namespace ComponentGenerator.Common.Models.Parameters
{
    internal abstract class ParameterModelBase
    {
        public string Name { get; }

        public string Type { get; }

        public bool IsOptional { get; }

        public ParameterModelBase(string name, string type,bool isOptional)
        {
            Name = name;
            Type = type;
            IsOptional = isOptional;
        }

        public override bool Equals(object obj)
        {
            return obj is ParameterModelBase @base &&
                   Name == @base.Name &&
                   Type == @base.Type &&
                   IsOptional == @base.IsOptional;
        }

        public override int GetHashCode()
        {
            int hashCode = -324767068;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Type);
            hashCode = hashCode * -1521134295 + IsOptional.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ParameterModelBase left, ParameterModelBase right)
        {
            return EqualityComparer<ParameterModelBase>.Default.Equals(left, right);
        }

        public static bool operator !=(ParameterModelBase left, ParameterModelBase right)
        {
            return !(left == right);
        }
    }
}