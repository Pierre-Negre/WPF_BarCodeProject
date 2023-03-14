using Dictionary.Enums;

namespace Dictionary.Models.Print
{
    /// <summary>
    /// Defines a Tag and holds a dictionary with all tags parameters
    /// </summary>
    public class TagModel
    {
        #region Class Definition
        public int PrintQty { get; set; }
        public double Height { get; set; }
        public double Width { get; set; }
        public double Margin { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Rotation { get; set; }
        public string Data { get; set; }

        public BarCodeModel Barcode { get; set; }
        public Texte Texte { get; set; }
        public Texte Emplacement { get; set; }

        public TagModel()
        {
            Height = 28.5;
            Width = 54;
            Margin = 3.5;
        }

        public TagModel(string textData, string BCData, string LocationData, int qty = 1)
        {
            Height = 28.5;
            Width = 54;
            Margin = 3.5;
            Tags.TryGetValue(LabelTypeEnum.Standard.ToString(), out var saveTag);
            if (saveTag is not null)
            {
                Barcode = saveTag.Barcode;
                Texte = saveTag.Texte;
                Emplacement = saveTag.Emplacement;
                Barcode.Data = BCData;
                Texte.Data = textData;
                Emplacement.Data = LocationData;
            }
            PrintQty = qty;
        }
        #endregion

        #region Dictionary
        /// <summary>
        /// Dictionary with all tags
        /// </summary>
        public static Dictionary<string, TagModel> Tags => new()
            {
                {
                    LabelTypeEnum.Standard.ToString(), new TagModel()
                    {

                        Texte = new Texte()
                        {
                            X = 20,
                            Y = 10,
                            Rotation = 0,
                            FontType = 2,
                            ScaleX = 1,
                            ScaleY = 1,
                            FontEffect = "N",
                        },
                        Barcode = new BarCodeModel()
                        {
                            X = 20,
                            Y = 50,
                            Degre = 0,
                            Rotation = 0,
                            BarNarrowWidth = 2,
                            BarWideWidth = 4,
                            BarType = "3",
                            HumanRead = "B",
                            Height = 90,
                        },
                        Emplacement = new Texte()
                        {
                            X = 245,
                            Y = 147,
                            Rotation = 0,
                            FontType = 2,
                            ScaleX = 1,
                            ScaleY = 1,
                            FontEffect = "N",
                        },

                    }
                }
            };
        #endregion
    }

    #region Child Classes

    /// <summary>
    /// Child class for Tag, defines the barcode section
    /// </summary>
    public sealed class BarCodeModel : TagModel
    {
        public int Degre { get; set; }
        public string BarType { get; set; }
        public int BarNarrowWidth { get; set; }
        public int BarWideWidth { get; set; }
        public string HumanRead { get; set; }
        public string AfficherLigne()
        {
            var ligne = string.Format("B{0},{1},{2},{3},{4},{5},{6},{7},\"{8}\"", X, Y, Degre, BarType, BarNarrowWidth, BarWideWidth, Height, HumanRead, Data);
            return ligne;
        }
    }

    /// <summary>
    /// Child class for Tag, defines the text sections
    /// </summary>
    public sealed class Texte : TagModel
    {
        public int FontType { get; set; }
        public int ScaleX { get; set; }
        public int ScaleY { get; set; }
        public string FontEffect { get; set; }
        public string AfficherLigne()
        {
            string? ligne;
            if (Data.Length>30)
            {
                // new line if text is too long
                ligne = string.Format("A{0},{1},{2},{3},{4},{5},{6},\"{7}\"", X, Y, Rotation, FontType, ScaleX, ScaleY, FontEffect, Data[..29]); 
                ligne += "\r\n";
                ligne += string.Format("A{0},{1},{2},{3},{4},{5},{6},\"{7}\"", X, Y+20, Rotation, FontType, ScaleX, ScaleY, FontEffect, Data[30..]);
            }
            else ligne = string.Format("A{0},{1},{2},{3},{4},{5},{6},\"{7}\"", X, Y, Rotation, FontType, ScaleX, ScaleY, FontEffect, Data);
            return ligne;
        }
    }
    #endregion
}