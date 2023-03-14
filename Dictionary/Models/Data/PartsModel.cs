namespace Dictionary.Models.Data
{
    /// <summary>
    /// Model for part View
    /// </summary>
    public sealed class PartsModel : BaseModel
    {
        public SearchModel Search { get; set; }

        public string StoreCode { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is PartsModel partsModel && partsModel.Libelle == Libelle && partsModel.Reference == Reference;
        }

        /// <summary>
        /// Child class, only used in PartsModel for groupBy
        /// </summary>
        public sealed class SearchModel
        {
            public string SLibelle { get; set; }
            public string SReference { get; set; }

            public SearchModel(string sLibelle, string sReference)
            {
                SLibelle = sLibelle;
                SReference = sReference;
            }

            public override bool Equals(object? obj)
            {
                var isEqual = false;
                var other = obj as SearchModel;
                if (other is not null)
                {
                    isEqual = other.SLibelle == this.SLibelle && other.SReference == this.SReference;
                }
                return isEqual;
            }
        }
    }
}