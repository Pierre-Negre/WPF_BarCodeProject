using Dictionary.Enums;

namespace Dictionary.Models.Print
{
    /// <summary>
    /// Defines a printer and holds a dictionary with all printer parameters
    /// </summary>
    public sealed class PrinterModel
    {
        #region Class Definition
        public PrinterModelEnum Model { get; set; }

        // INTERMEC-PC4 utilise le language EPL2.
        // Ce language est aussi disponible avec d'autres imprimantes comme Zebra.
        // N = vide le cache, q = largeur étiquette, Q = hauteur+marge étiquette, R = point de référence (x,y), j = décalage pour découpage après impression
        public string PrinterSystemCommand { get; set; }
        public int PrintQty { get; set; }
        public string PrintCommand()
        {
            return "P" + PrintQty + "\n";
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// Dictionary with all printers
        /// </summary>
        public static Dictionary<PrinterModelEnum, PrinterModel> Printers =>
            new()
            {
                {
                    PrinterModelEnum.INTERMEC_PC4, new PrinterModel
                    {
                        Model = PrinterModelEnum.INTERMEC_PC4,
                        PrinterSystemCommand = "N\nq405\nQ203,25\nJF\nZB\nD10\nS2",
                        PrintQty = 1
                    }
                },
            };
        #endregion
    }
}
