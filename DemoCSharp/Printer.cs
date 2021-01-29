using System;
using System.Drawing.Printing;
using PavoCardSDK;
using System.Drawing;

namespace DemoCSharp
{
    public class Printer
    {

        static int ORIENTATION = 2; //1 portrait, 2 landscape

        //Form1_Load
        public static void PrintInit()
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (String.Compare(strPrinter, 0, "HiTi CS-2", 0, 9) == 0)
                {
                    System.Console.WriteLine("Printer HiTi CS-2 connected");
                }
            }
            uint nBufferSize = 0;
            if (PavoApi.PAVO_EnumUSBCardPrinters(null, ref nBufferSize) == 0)
            {
                if (nBufferSize != 0)
                {
                    byte[] printerBuf = new byte[nBufferSize];
                    PavoApi.PAVO_EnumUSBCardPrinters(printerBuf, ref nBufferSize);
                }
            }
        }

        //buttonPrintByDriver_Click
        public static void Print()
        {
            PAVO_JOB_PROPERTY JobProp = new PAVO_JOB_PROPERTY();
            JobProp.dwFieldFlag = PavoApi.FF_CARD_TYPE | PavoApi.FF_FLAGS | PavoApi.FF_PARENT_HWND | PavoApi.FF_ORIENTATION
                     | PavoApi.FF_COPIES | PavoApi.FF_DUPLEX | PavoApi.FF_RIBBON_TYPE | PavoApi.FF_PRINT_COLOR;
            //JobProp.hParentWnd = this.Handle;
            JobProp.shOrientation = (short)(ORIENTATION);
            JobProp.shCopies = 1;

            //JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKO;  
            //JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_HALF_YMCKO;
            //JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKOK;
            //JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_KO; 
            JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_K;

            // Card type: 0)Blank Card, 1)6pin smart chip, 2)8pin smart chip
            JobProp.dwCardType = (uint)0;

            // FRONT SIDE PRINTING CONFIGS:
            JobProp.byDuplex = 1;
            JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
            JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_FRONT;

            // BACK SIDE PRINTING CONFIGS:
            //JobProp.byDuplex |= 2;
            //m_nPageToPrint++;
            //JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
            //JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_BACK;

            //CREATE PRINT JOB:
            PrintDocument pd = new PrintDocument();

            pd.PrinterSettings.PrinterName = "HiTi CS-200e";
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DocumentName = "Card Print Job";

            PavoApi.PAVO_ApplyJobSetting(pd.PrinterSettings, ref JobProp);

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Print error");
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }

            pd.PrintPage -= pd_PrintPage;
        }

        public static void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Image colorImg = null;
            Rectangle rect;

            uint dwRet = 0;
            IntPtr hPrinterDC;
            uint dwDataTypeK = 0;
            Bitmap bmpK8;

            rect = new Rectangle(0, 0, 1014, 642);
            e.Graphics.PageUnit = GraphicsUnit.Pixel;

            //---------------------------------------------
            hPrinterDC = e.Graphics.GetHdc();

            //SEND RESIN K ----------------------
            dwDataTypeK = PavoApi.PAVO_DATA_RESIN_FRONT;
            if (dwDataTypeK != 0)
            {
                bmpK8 = (Bitmap)Bitmap.FromFile(@"C:\1014x642_k.bmp");
                dwRet = PavoApi.PAVO_SetExtraDataToHDC(hPrinterDC, dwDataTypeK, bmpK8);
            }

            e.Graphics.ReleaseHdc(hPrinterDC);
            //---------------------------------------------
            e.HasMorePages = false;
        }

    }

}
