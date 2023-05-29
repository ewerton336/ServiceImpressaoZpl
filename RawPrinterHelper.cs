using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

public class RawPrinterHelper
{
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool OpenPrinter(string pPrinterName, out IntPtr phPrinter, IntPtr pDefault);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool StartDocPrinter(IntPtr hPrinter, int level, ref DOC_INFO_1 di);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool EndDocPrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool StartPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool EndPagePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool ClosePrinter(IntPtr hPrinter);

    [DllImport("winspool.drv", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int DocumentProperties(IntPtr hwnd, IntPtr hPrinter, string pDeviceName, IntPtr pDevModeOutput, IntPtr pDevModeInput, int fMode);

    public static bool SendStringToPrinter(string printerName, string data)
    {
        IntPtr pBytes;
        int dwCount;
        int dwWritten = 0;

        dwCount = data.Length;
        pBytes = Marshal.StringToCoTaskMemUni(data);

        IntPtr hPrinter;
        bool success = false;

        if (OpenPrinter(printerName, out hPrinter, IntPtr.Zero))
        {
            var di = new DOC_INFO_1
            {
                pDocName = "Raw Printer Data",
                pOutputFile = null,
                pDataType = "RAW"
            };

            if (StartDocPrinter(hPrinter, 1, ref di))
            {
                if (StartPagePrinter(hPrinter))
                {
                    success = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                    EndPagePrinter(hPrinter);
                }
                EndDocPrinter(hPrinter);
            }
            ClosePrinter(hPrinter);
        }

        Marshal.FreeCoTaskMem(pBytes);
        return success;
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public class DOC_INFO_1
{
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDocName;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pOutputFile;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDataType;
}
