namespace ComponentGenerator.Common.Models.Parameters
{
    internal class AliasCollectionParameterModel : ParameterModelBase
    {
        public AliasCollectionParameterModel(string name, string type, bool isOptional) : base(name, type, isOptional)
        {
        }

        public override bool Equals(object obj)
        {
            return obj is AliasCollectionParameterModel model &&
                   base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return 624022166 + base.GetHashCode();
        }
    }
}