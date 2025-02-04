namespace ComponentGenerator.Common.Models.Parameters
{
    internal class AliasParameterModel : ParameterModelBase
    {
        public AliasParameterModel(string name, string type, bool isOptional) : base(name, type, isOptional)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is AliasParameterModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }
    }
}