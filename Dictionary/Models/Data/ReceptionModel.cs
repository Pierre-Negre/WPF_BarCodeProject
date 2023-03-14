namespace Dictionary.Models.Data
{
    /// <summary>
    /// Model for reception View
    /// </summary>
    public sealed class ReceptionModel : BaseModel
    {
        public ReceptionOrderModel Order { get; set; }
        public bool ExceedLimit { get; set; }


        /// <summary>
        /// Child class, only used in ReceptionModel for groupBy
        /// </summary>
        public sealed class ReceptionOrderModel
        {
            public string OrderID { get; set; }
            public string Provider { get; set; }

            public ReceptionOrderModel(string orderID, string provider)
            {
                OrderID = orderID;
                Provider = provider;
            }

            public override bool Equals(object? obj)
            {
                var isEqual = false;
                var other = obj as ReceptionOrderModel;
                if (other is not null)
                {
                    isEqual = other.OrderID == this.OrderID && other.Provider == this.Provider;
                }
                return isEqual;
            }
        }
    }
}