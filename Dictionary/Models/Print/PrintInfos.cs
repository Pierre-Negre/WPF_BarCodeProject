namespace Dictionary.Models.Print
{
    /// <summary>
    /// Defines print parameters
    /// </summary>
    public sealed class PrintInfos
    {
        public string TextLine { get; set; }
        public string BarCodeLine { get; set; }
        public string Emplacement { get; set; }
        public int PrintQty { get; set; }
    }
}
