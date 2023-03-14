using Dictionary.Enums;
using Dictionary.Models.Print;
using System.Runtime.InteropServices;

namespace WPF_Front.Common
{
    /// <summary>
    /// Manages all printer connection and commands
    /// </summary>
    internal static class PrinterLogic
    {
        #region DllImports /*do not alter*/

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        #endregion

        /// <summary>
        /// Generate the command line & calls SendToPrinter()
        /// </summary>
        /// <param name="datas"></param>
        internal static int Print(IEnumerable<PrintInfos> datas)
        {
            string command = "";
            if (datas == null || !datas.Any()) return -1;
            foreach (var data in datas)
            {
                var tag = new TagModel(data.TextLine, data.BarCodeLine, data.Emplacement);
                PrinterModel.Printers.TryGetValue(PrinterModelEnum.INTERMEC_PC4, out var selectedPrinter);
                if (selectedPrinter is null) return -2;
                command += selectedPrinter.PrinterSystemCommand;
                command += "\r\n" + tag.Texte.AfficherLigne();
                command += "\r\n" + tag.Barcode.AfficherLigne();
                if (tag.Emplacement.Data is null) tag.Emplacement.Data = string.Empty;
                command += "\r\n" + tag.Emplacement.AfficherLigne();
                selectedPrinter.PrintQty = data.PrintQty;
                command += "\r\n" + selectedPrinter.PrintCommand();
            }

            command += "\r\n";
            return SendToPrinter(XmlLogic.SyncXmlValue(XmlLogic.PrinterNameXmlNode), command);
        }

        /// <summary>
        /// Cancel all ongoing print commands
        /// </summary>
        /// <returns></returns>
        internal static int Cancel()
        {
            var command = "~JA";
            return SendToPrinter(XmlLogic.SyncXmlValue(XmlLogic.PrinterNameXmlNode), command);
        }

        #region privates printer ressources
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private sealed class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        /// <summary>
        /// When the function is given a printer name and an unmanaged array of bytes, the function sends those bytes to the print queue.
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="command"></param>
        /// <returns>true on success, false on failure</returns>
        private static int SendToPrinter(string printerName, string command)
        {

            // Assume that the printer is expecting ANSI text, and then convert the string to ANSI text.
            IntPtr bytes = Marshal.StringToCoTaskMemAnsi(command);
            bool success = false;
            int Code = 0;
            // Open the printer.
            if (OpenPrinter(printerName.Normalize(), out var printer, IntPtr.Zero))
            {
                if (StartDocPrinter(printer, 1, new DOCINFOA()))
                {
                    if (StartPagePrinter(printer))
                    {
                        // Write your bytes.
                        success = WritePrinter(printer, bytes, command.Length, out _);
                        EndPagePrinter(printer);
                    }
                    EndDocPrinter(printer);
                }
                ClosePrinter(printer);
            }

            if (!success)
            {
                Code = Marshal.GetLastWin32Error();
            }
            Marshal.FreeCoTaskMem(bytes);
            return Code;
        }
        #endregion
    }
}