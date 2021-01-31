using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Threading;

//using System.Runtime.InteropServices;


using PavoCardSDK;
using CS2xxRFID;
using System.Net.WebSockets;
using System.IO;
using System.Drawing.Imaging;
using System.Linq;

namespace DemoCSharp
{
    public struct RECT
    {
        long left;
        long top;
        long right;
        long bottom;
    }

    public partial class Form1 : Form, SocketClient.IListener
    {
        int m_nPageToPrint;
        int m_nPagePrinted;
        readonly SocketClient client;
        string filePath = @"C:\Users\Public\card.bmp";

        //[DllImport("YLE402S.dll")]
        //static extern int GetCardNo(StringBuilder InStr, StringBuilder RcStr);

        public Form1()
        {
            InitializeComponent();
            client = new SocketClient(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (String strPrinter in PrinterSettings.InstalledPrinters)
            {
                if (String.Compare(strPrinter, 0, "HiTi CS-2", 0, 9) == 0)
                    comboBoxPrinterQueue.Items.Add(strPrinter);
                else
                {
                    if (String.Compare(strPrinter, 0, "CIAAT CTC", 0, 9) == 0)
                        comboBoxPrinterQueue.Items.Add(strPrinter);
                    else
                    {
                        if (String.Compare(strPrinter, 0, "Fagoo P28", 0, 9) == 0)
                            comboBoxPrinterQueue.Items.Add(strPrinter);
                    }
                }
            }
            if (comboBoxPrinterQueue.Items.Count == 0)
            {
                radioButton1.Enabled = false;
                radioButton2.Checked = true;
            }
            else
                comboBoxPrinterQueue.SelectedIndex = 0;

            comboBoxCoercivity.SelectedIndex = 1;

            comboBoxOrientation.SelectedIndex = 1;
            comboBoxRibbon.SelectedIndex = 0;
            comboBoxCardType.SelectedIndex = 0;
            comboBoxFrontSide.SelectedIndex = 3;
            comboBoxBackSide.SelectedIndex = 0;

             uint nBufferSize = 0;
             if (PavoApi.PAVO_EnumUSBCardPrinters(null, ref nBufferSize) == 0)
             {
                 if (nBufferSize != 0)
                 {
                     byte[] printerBuf = new byte[nBufferSize];

                     PavoApi.PAVO_EnumUSBCardPrinters(printerBuf, ref nBufferSize);

                     textBoxUSBLink.Text = System.Text.Encoding.UTF8.GetString(printerBuf);

                     radioButton3.Checked = true;
                 }
                 else
                     radioButton3.Enabled = false;
             }
             else
                 radioButton3.Enabled = false;

            //CreateService();
        }

        private void buttonBrowseColor1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxColor1.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseK1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxK1.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseO1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxO1.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseColor2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxColor2.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseK2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxK2.Text = openFileDialog1.FileName;
            }
        }

        private void buttonBrowseO2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files(*.BMP;*.JPG)|*.BMP;*.JPG";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxO2.Text = openFileDialog1.FileName;
            }
        }

        private void buttonPrintByDriver_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {
                MessageBox.Show("Please select printer queue.", "Print By Driver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PAVO_JOB_PROPERTY JobProp = new PAVO_JOB_PROPERTY();

            m_nPageToPrint = 0;
            m_nPagePrinted = 0;

            JobProp.dwFieldFlag = PavoApi.FF_CARD_TYPE | PavoApi.FF_FLAGS | PavoApi.FF_PARENT_HWND | PavoApi.FF_ORIENTATION
                     | PavoApi.FF_COPIES | PavoApi.FF_DUPLEX | PavoApi.FF_RIBBON_TYPE | PavoApi.FF_PRINT_COLOR;

            JobProp.hParentWnd = this.Handle;

            JobProp.shOrientation = (short)(comboBoxOrientation.SelectedIndex + 1);
            JobProp.shCopies = 1;

            switch (comboBoxRibbon.SelectedIndex)
            {
                case 0: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKO; break;
                case 1: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_K; break;
                case 2: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_HALF_YMCKO; break;
                case 3: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKOK; break;
                case 4: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_KO; break;
            }

            JobProp.dwCardType = (uint)comboBoxCardType.SelectedIndex;

            JobProp.dwFlags = 0;
            if (checkBox1.Checked)
                JobProp.dwFlags = PavoApi.PAVO_FLAG_NOT_SHOW_ERROR_MSG_DLG;
            if (checkBox2.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_WAIT_MSG_DONE;
            if (checkBox3.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_NOT_SHOW_CLEAN_MSG;
            if (checkBox4.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_WATCH_JOB_PRINTED;
            if (checkBox6.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_NOT_EJECT_CARD_AFTER_PRINTED;
            if (checkBox7.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_MOVE_CARD_TO_STANDBY_AFTER_PRINTED;

            if (checkBox5.Checked)
                JobProp.byPrintAs230e = 1;
            else
                JobProp.byPrintAs230e = 0;

            if (comboBoxFrontSide.SelectedIndex > 0)
            {
                JobProp.byDuplex = 1;
                m_nPageToPrint++;
            }
            if (comboBoxBackSide.SelectedIndex > 0)
            {
                JobProp.byDuplex |= 2;
                m_nPageToPrint++;
            }

            if (comboBoxFrontSide.SelectedIndex == 1 || comboBoxFrontSide.SelectedIndex == 3)
                JobProp.byPrintColor = 1;
            if (comboBoxBackSide.SelectedIndex == 1 || comboBoxBackSide.SelectedIndex == 3)
                JobProp.byPrintColor |= 2;

            if (checkBoxK1.Checked && (comboBoxFrontSide.SelectedIndex == 2 || comboBoxFrontSide.SelectedIndex == 3))
            {
                JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
                JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_FRONT;
            }
            if (checkBoxK2.Checked && (comboBoxBackSide.SelectedIndex == 2 || comboBoxBackSide.SelectedIndex == 3))
            {
                JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
                JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_BACK;
            }

            //create print job
            PrintDocument pd = new PrintDocument();

            pd.PrinterSettings.PrinterName = comboBoxPrinterQueue.Text;
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DocumentName = "DemoCSharp Print Job";

            PavoApi.PAVO_ApplyJobSetting(pd.PrinterSettings, ref JobProp);

            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "列印失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }

            pd.PrintPage -= pd_PrintPage;
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Image colorImg = null;
            Rectangle rect;

            uint dwRet = 0;
            IntPtr hPrinterDC;
            uint dwDataTypeK = 0;
            Bitmap bmpK8;

            if (comboBoxOrientation.SelectedIndex == 0)//portrait
                rect = new Rectangle(0, 0, 642, 1014);
            else//landscape
                rect = new Rectangle(0, 0, 1014, 642);

            e.Graphics.PageUnit = GraphicsUnit.Pixel;

            //---------------------------------------------
            hPrinterDC = e.Graphics.GetHdc();

            //send resin K ----------------------
            if (m_nPageToPrint == 1)
            {
                if (comboBoxFrontSide.SelectedIndex > 0 && checkBoxK1.Checked)
                    dwDataTypeK = PavoApi.PAVO_DATA_RESIN_FRONT;
                else if (comboBoxBackSide.SelectedIndex > 0 && checkBoxK2.Checked)
                    dwDataTypeK = PavoApi.PAVO_DATA_RESIN_BACK;
            }
            else if (m_nPageToPrint == 2)
            {
                if (m_nPagePrinted == 0)//front side
                {
                    if (checkBoxK1.Checked)
                        dwDataTypeK = PavoApi.PAVO_DATA_RESIN_FRONT;
                }
                else//back side
                {
                    if (checkBoxK2.Checked)
                        dwDataTypeK = PavoApi.PAVO_DATA_RESIN_BACK;
                }
            }

            if (dwDataTypeK != 0)
            {
                if (dwDataTypeK == PavoApi.PAVO_DATA_RESIN_FRONT)
                    bmpK8 = (Bitmap)Bitmap.FromFile(textBoxK1.Text);
                else
                    bmpK8 = (Bitmap)Bitmap.FromFile(textBoxK2.Text);
                dwRet = PavoApi.PAVO_SetExtraDataToHDC(hPrinterDC, dwDataTypeK, bmpK8);
            }

            e.Graphics.ReleaseHdc(hPrinterDC);
            //---------------------------------------------

            //send color image ============================
            if (m_nPageToPrint == 1)
            {
                if (checkBoxColor1.Checked)
                {
                    //InsertText("Front Side");
                    colorImg = Image.FromFile(textBoxColor1.Text);
                }
                else if (checkBoxColor2.Checked)
                {
                    //InsertText("Back Side");
                    colorImg = Image.FromFile(textBoxColor2.Text);
                }

                if ( colorImg != null )
                e.Graphics.DrawImage(colorImg, rect);

                // Create string to draw.
                String drawString = "Sample Text";

                // Create font and brush.
                Font drawFont = new Font("Arial", 16);
                SolidBrush drawBrush = new SolidBrush(Color.Black);
                    
                // Create point for upper-left corner of drawing.
                PointF drawPoint = new PointF(150.0F, 150.0F);

                // Draw string to screen.
                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
            }
            else if (m_nPageToPrint == 2)
            {
                if (m_nPagePrinted == 0)
                {
                    if (checkBoxColor1.Checked)
                    {
                        //InsertText("Front Side");
                        colorImg = Image.FromFile(textBoxColor1.Text);
                    }
                }
                else
                {
                    if (checkBoxColor2.Checked)
                    {
                        //InsertText("Back Side");
                        colorImg = Image.FromFile(textBoxColor2.Text);
                    }
                }

                if (colorImg != null)
                e.Graphics.DrawImage(colorImg, rect);
            }

            m_nPagePrinted++;

            if (m_nPageToPrint == m_nPagePrinted)
                e.HasMorePages = false;
            else
                e.HasMorePages = true;
        }

        // Override the default WndProc behavior to examine messages.
        protected override void WndProc(ref Message msg)
        {
            switch (msg.Msg)
            {
                // If message is of interest, invoke the method on the form that
                // functions as a callback to perform actions in response to the message.
                case PavoApi.WM_PAVO_PRINTER:
                    {
                        switch ((int)msg.WParam)
                        {
                            case PavoApi.MSG_JOB_BEGIN:
                                //MessageBox.Show(this, "Job is begining by driver.", "WndProc");
                                break;

                            case PavoApi.MSG_JOB_END:
                                MessageBox.Show(this, "Job is processed by driver.", "WndProc");
                                break;

                            case PavoApi.MSG_JOB_CANCELED:
                                MessageBox.Show(this, "Job is canceled by driver.", "WndProc");
                                break;

                            case PavoApi.MSG_DEVICE_STATUS:
                                if ((uint)msg.LParam != PavoApi.PAVO_DS_PRINTING && (uint)msg.LParam != PavoApi.PAVO_DS_PROCESSING_DATA && (uint)msg.LParam != PavoApi.PAVO_DS_SENDING_DATA)
                                {
                                    MessageBox.Show(this, "Error happened!!", "WndProc");
                                }
                                break;

                            case PavoApi.MSG_JOB_PRINTED:
                                MessageBox.Show(this, "Job is printed completely by printer.", "WndProc");
                                break;
                        }
                    }
                    break;
            }

            // Call the base WndProc method
            // to process any messages not handled.
            base.WndProc(ref msg);
        }

        private void buttonDirectPrint_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            string msg;

            string printer = null;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;
          
            HITI_CARD_PRINT_PARAMETER JobPara = new HITI_CARD_PRINT_PARAMETER();
            HITI_HEATING_ENERGY HeatEnergy = new HITI_HEATING_ENERGY();

            Bitmap BmpColor1 = null, BmpK1 = null, BmpO1 = null;
            Bitmap BmpColor2 = null, BmpK2 = null, BmpO2 = null;

            if (checkBoxColor1.Checked)
                BmpColor1 = (Bitmap)Bitmap.FromFile(textBoxColor1.Text);
            if (checkBoxK1.Checked)
                BmpK1 = (Bitmap)Bitmap.FromFile(textBoxK1.Text);
            if (checkBoxO1.Checked)
                BmpO1 = (Bitmap)Bitmap.FromFile(textBoxO1.Text);
            if (checkBoxColor2.Checked)
                BmpColor2 = (Bitmap)Bitmap.FromFile(textBoxColor2.Text);
            if (checkBoxK2.Checked)
                BmpK2 = (Bitmap)Bitmap.FromFile(textBoxK2.Text);
            if (checkBoxO2.Checked)
                BmpO2 = (Bitmap)Bitmap.FromFile(textBoxO2.Text);

            JobPara.byOrientation = (byte)(comboBoxOrientation.SelectedIndex + 1);
            JobPara.FrontBGR = BmpColor1;
            JobPara.FrontK = BmpK1;
            JobPara.FrontO = BmpO1;
            JobPara.BackBGR = BmpColor2;
            JobPara.BackK = BmpK2;
            JobPara.BackO = BmpO2;

            dwRet = PavoApi.PAVO_PrintOneCard(printer, JobPara, HeatEnergy);

            if (BmpColor1 != null)
                BmpColor1.Dispose();
            if (BmpK1 != null)
                BmpK1.Dispose();
            if (BmpO1 != null)
                BmpO1.Dispose();
            if (BmpColor2 != null)
                BmpColor2.Dispose();
            if (BmpK2 != null)
                BmpK2.Dispose();
            if (BmpO2 != null)
                BmpO2.Dispose();

            if (dwRet != 0)//some error happen
            {
                msg = String.Format("dwRet = {0}", dwRet);
                MessageBox.Show(this, msg, "PAVO_PrintOneCard");
                return;
            }

            //4. Optional
            //If you need to do another encoding, please wait printer to print card completely.
            
            Thread.Sleep(5000);//wait for printer firmware to process data
            uint dwStatus = 0;

            while (true)
            {
                dwRet = PavoApi.PAVO_CheckPrinterStatus(printer, ref dwStatus);

                if (dwStatus != PavoApi.PAVO_DS_BUSY && dwStatus != PavoApi.PAVO_DS_PRINTING)
                    break;

                Thread.Sleep(1000);
            }

            if (dwStatus != 0)
            {

                msg = String.Format("Printer error =>  0x{0:X8}", dwStatus);
                MessageBox.Show(this, msg, "PAVO_PrintOneCard");
            }
            else
                MessageBox.Show(this, "PAVO_PrintOneCard() return success.", "OnBtnDirectPrint");

        }

        private void buttonMoveCard_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            uint dwPosition = 0;
            string PosString;
            string msg;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            switch (comboBoxMoveCardPos.SelectedIndex)
            {
                default: dwPosition = 0; PosString = ""; break;
                case 0: dwPosition = PavoApi.MOVE_CARD_TO_IC_ENCODER; PosString = "Contact Encoder"; break;
                case 1: dwPosition = PavoApi.MOVE_CARD_TO_RFID_ENCODER; PosString = "Contactless Encoder"; break;
                case 2: dwPosition = PavoApi.MOVE_CARD_TO_HOPPER; PosString = "Output Hopper"; break;
                case 3: dwPosition = PavoApi.MOVE_CARD_TO_REJECT_BOX; PosString = "Reject Box"; break;
                case 4: dwPosition = PavoApi.MOVE_CARD_TO_FLIPPER; PosString = "Flipper"; break;
                case 5: dwPosition = PavoApi.MOVE_CARD_TO_PRINT_FROM_FLIPPER; PosString = "Print Position"; break;
                case 6: dwPosition = PavoApi.MOVE_CARD_TO_STANDBY_POSITION; PosString = "Standby Position"; break;
	        }

            if (dwPosition == 0)
                return;

            dwRet = PavoApi.PAVO_MoveCard(printer, dwPosition);

            msg = String.Format("Move card to {0} finished.\n\ndwRet = 0x{1:X8}", PosString, dwRet);
            MessageBox.Show(this, msg, "OnBtnMoveCard");
        }

        private void buttonGetDeviceInfo_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            int nInfoType = 0;
            byte[] dataBuf = new byte[256];
            uint dwBufSize = 256;
            string temp;
            uint dwValue1 = 0, dwValue2 = 0;

            string msg;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            nInfoType = comboBoxDeviceInfoType.SelectedIndex;

            if (nInfoType == 0)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_MFG_SERIAL, dataBuf, ref dwBufSize);
                temp = System.Text.Encoding.Unicode.GetString(dataBuf);
                msg = String.Format("Printer serial = {0}\n dwRet = 0x{1:X8}", temp, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 1)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_MODEL_NAME, dataBuf, ref dwBufSize);
                temp = System.Text.Encoding.Unicode.GetString(dataBuf);
                msg = String.Format("Printer model = {0}\n\ndwRet = 0x{1:X8}", temp, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 2)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_FIRMWARE_VERSION, dataBuf, ref dwBufSize);
                temp = System.Text.Encoding.Unicode.GetString(dataBuf);
                msg = String.Format("Firmware version = {0}\n\ndwRet = 0x{1:X8}", temp, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 3)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_RIBBON_INFO, dataBuf, ref dwBufSize);
                dwValue1 = BitConverter.ToUInt32(dataBuf, 0);
                dwValue2 = BitConverter.ToUInt32(dataBuf, 4);

                switch(dwValue1)
                {
                    case PavoApi.PAVO_RIBBON_TYPE_YMCKO: temp = "YMCKO"; break;
                    case PavoApi.PAVO_RIBBON_TYPE_K: temp = "Resin K"; break;
                    case PavoApi.PAVO_RIBBON_TYPE_KO: temp = "KO"; break;
                    case PavoApi.PAVO_RIBBON_TYPE_YMCKOK: temp = "YMCKOK"; break;
                    case PavoApi.PAVO_RIBBON_TYPE_HALF_YMCKO: temp = "1/2 YMCKO"; break;
                    case PavoApi.PAVO_RIBBON_TYPE_YMCKFO: temp = "YMCKFO"; break;
                    default:
                    case 255: temp = "Ribbon is missing or not supported"; break;
                }

                msg = String.Format("Ribbon type = {0} ({1})\n\nRemain count = {2}\n\ndwRet = 0x{3:X8}", dwValue1, temp, dwValue2, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 4)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_PRINT_COUNT, dataBuf, ref dwBufSize);
                dwValue1 = BitConverter.ToUInt32(dataBuf, 0);
                msg = String.Format("Printed card = {0}\n\ndwRet = 0x{1:X8}", dwValue1, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 5)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_CARD_POSITION, dataBuf, ref dwBufSize);
                dwValue1 = BitConverter.ToUInt32(dataBuf, 0);

                switch (dwValue1)
                {
                    default:
                    case 0: temp = "Out of printer"; break;
                    case 1: temp = "Start printing position"; break;
                    case 2: temp = "Mag out position"; break;
                    case 3: temp = "Mag in position"; break;
                    case 4: temp = "Contact encoder position"; break;
                    case 5: temp = "Contactless encoder position"; break;
                    case 6: temp = "Flipper position"; break;
                    case 7: temp = "Card jam"; break;
                }

                msg = String.Format("Card position = {0}\n\n{1}\n\ndwRet = 0x{2:X8}", dwValue1, temp, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 6)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_UNCLEAN_COUNT, dataBuf, ref dwBufSize);
                dwValue1 = BitConverter.ToUInt32(dataBuf, 0);

                msg = String.Format("Unclean count = {0}\n\ndwRet = 0x{1:X8}", dwValue1, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 7)
            {
                string mystring = "D:/";
                System.Text.UnicodeEncoding encoding = new System.Text.UnicodeEncoding();
                encoding.GetBytes(mystring, 0, mystring.Length, dataBuf, 0);
                //System.Text.Encoding.(mystring, 0, mystring.Length, dataBuf, 0);

                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_SAVE_MAINTENANCE_INFO, dataBuf, ref dwBufSize);

                string myRetString = System.Text.Encoding.Unicode.GetString(dataBuf);
                msg = String.Format("Save maintenance info file to \n\n{0}, \n\ndwRet = 0x{1:X8}", myRetString, dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
            else if (nInfoType == 8)
            {
                dwRet = PavoApi.PAVO_GetDeviceInfo(printer, PavoApi.PAVO_DEVINFO_ATTACHED_MODULE,  dataBuf, ref dwBufSize);
                dwValue1 = BitConverter.ToUInt32(dataBuf, 0);

                msg = String.Format("Attached module flag = {0},\n\nFlipper is attached: {1}, \n\ndwRet = 0x{2:X8}", dwValue1, (dwValue1 & 0x00000001) > 0 ? "Yes" : "No", dwRet);
                MessageBox.Show(this, msg, "Get Device Info");
            }
        }

        private void buttonDoCommand_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            uint dwCommand = 0;
            string msg;
            uint dwStatus = 0;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            switch (comboBoxCommand.SelectedIndex)
            {
                default: dwCommand = 0; break;
                case 0: dwCommand = PavoApi.PAVO_COMMAND_RESET_PRINTER; break;
                case 1: dwCommand = PavoApi.PAVO_COMMAND_CLEAN_CARD_PATH; break;
                case 2: dwCommand = PavoApi.PAVO_COMMAND_RESET_PRINT_COUNT; break;
                case 3: dwCommand = PavoApi.PAVO_COMMAND_FLIP_CARD; break;
                case 4: dwCommand = PavoApi.PAVO_COMMAND_AUTO_FEED_FROM_FLIPPER_ON; break;
                case 5: dwCommand = PavoApi.PAVO_COMMAND_AUTO_FEED_FROM_FLIPPER_OFF; break;
                case 6: dwCommand = PavoApi.PAVO_COMMAND_RESET_PRINTER_CLEAR_JAM; break;
                case 7: dwCommand = PavoApi.PAVO_COMMAND_TIGHTEN_RIBBON; break;
                case 8: dwCommand = PavoApi.PAVO_COMMAND_ERASE_CARD_BY_DRIVER; break;
                case 9: dwCommand = PavoApi.PAVO_COMMAND_DETECT_CARD_EMPTY; break;
            }

            if (dwCommand == 0)
                return;

            dwRet = PavoApi.PAVO_DoCommand(printer, dwCommand);

            if (comboBoxCommand.SelectedIndex == 1)//clean card path
            {
                //check if there is error
                dwStatus = PavoApi.PAVO_DS_BUSY;
                while (dwStatus == PavoApi.PAVO_DS_BUSY)
                {
                    dwRet = PavoApi.PAVO_CheckPrinterStatus(printer, ref dwStatus);

                    if (dwStatus == PavoApi.PAVO_DS_BUSY)
                        Thread.Sleep(1000);
                }

                if (dwStatus != 0 && dwStatus != PavoApi.PAVO_DS_PRINTING)
                {
                    msg = String.Format("PAVO_DoCommand() finished. Printer error =>\n\ndwRet = 0x{0:X8}", dwStatus);
                }
                else
                {
                    msg = String.Format("PAVO_DoCommand() finished. Card is not empty!,\n\ndwRet = 0x{0:X8}", dwStatus);
                }
            }
            else if (comboBoxCommand.SelectedIndex == 9)//detect card empty
            {
                //check if there is error
                dwStatus = PavoApi.PAVO_DS_BUSY;
                while (dwStatus == PavoApi.PAVO_DS_BUSY)
                {
                    dwRet = PavoApi.PAVO_CheckPrinterStatus(printer, ref dwStatus);

                    if (dwStatus == PavoApi.PAVO_DS_BUSY)
                        Thread.Sleep(1000);
                }

                if (dwStatus != 0 && dwStatus != PavoApi.PAVO_DS_PRINTING)
                {
                    msg = String.Format("PAVO_DoCommand() finished. Printer error =>\n\ndwRet = 0x{0:X8}", dwStatus);
                }
                else
                {

                    msg = String.Format("PAVO_DoCommand() finished. Card is not empty!,\n\ndwRet = 0x{0:X8}", dwStatus);
                }
            }
            else
            {
                
                msg = String.Format("PAVO_DoCommand() finished.\n\ndwRet = 0x{0:X8}", dwRet);
            }
            
            MessageBox.Show(this, msg, "OnBtnDoCommand");
        }

        private void buttonReadMagTrackData_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            string msg;

            textBoxT1.Text = "";
            textBoxT2.Text = "";
            textBoxT3.Text = "";

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            string T1, T2, T3;
            MAG_TRACK_DATA2 TrackData = new MAG_TRACK_DATA2();

            TrackData.byTrackFlag = 0;
            if (checkBoxT1.Checked)
                TrackData.byTrackFlag |= 1;
            if (checkBoxT2.Checked)
                TrackData.byTrackFlag |= 0x12;
            if (checkBoxT3.Checked)
                TrackData.byTrackFlag |= 4;

            if (comboBoxCoercivity.SelectedIndex == 0)
                TrackData.byCoercivity = 0;
            else
                TrackData.byCoercivity = 1;

            TrackData.szTrack1 = new byte[256];
            TrackData.szTrack2 = new byte[256];
            TrackData.szTrack3 = new byte[256];
            dwRet = PavoApi.PAVO_ReadMagTrackData(printer, 0, ref TrackData);

            /*char RcBuf[40];
       
		    HINSTANCE hLibrary=LoadLibrary(_T("YLE402S.dll"));
     
		    if(hLibrary)
		    {
			    _YLE402S_GetCardNo YLE402S_GetCardNo  = (_YLE402S_GetCardNo)GetProcAddress(hLibrary,  _T("YLE402S_GetCardNo"));
       
    			memset(RcBuf, 0x00, sizeof(RcBuf));
       
	    		if(YLE402S_GetCardNo(TrackDataR.szTrack2, RcBuf) == YLE402S_OK) {
		    		MessageBox(Temp, _T("YLE402S_GetCardNo OK"), MB_OK);
			    }
			    else {
				    MessageBox(Temp, _T("YLE402S_GetCardNo failed"), MB_OK);
			    }
   
			    FreeLibrary(hLibrary);      
			    m_T2 = RcBuf;
		    }
    		else
	    	{
		        CString				test;
			    test.Format(_T("LoadLibrary YLE402S.dll failed, Error Code = %d"), GetLastError());
			
			    MessageBox(test, NULL, MB_OK);

			    m_T2 = TrackDataR.szTrack2;
		    }*/

            

            /*StringBuilder OutSB=new StringBuilder(TrackData.szTrack2);
			StringBuilder InSB=new StringBuilder();
			if (GetCardNo(OutSB, InSB) != 0)
			{
				//this.Text = "解码失败!";
                MessageBox.Show(this,  "解码失败!", "PAVO_ReadMagTrackData");
				return;
			}*/

            T1 = System.Text.Encoding.ASCII.GetString(TrackData.szTrack1);
            T2 = System.Text.Encoding.ASCII.GetString(TrackData.szTrack2);
            //T2 = InSB.ToString();

            T3 = System.Text.Encoding.ASCII.GetString(TrackData.szTrack3);

            textBoxT1.Text = T1;
            textBoxT2.Text = T2;
            textBoxT3.Text = T3;

            //msg = String.Format("Track1 = {0}\nTrack2 = {1}\nTrack3 = {2}\n\ndwRet = {3}", T1, T2, T3, dwRet);
            msg = String.Format("dwRet = {0}", dwRet);
            MessageBox.Show(this, msg, "PAVO_ReadMagTrackData");
        }

        private void buttonWriteMagTrackData_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            string msg;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text; ;

            MAG_TRACK_DATA2 TrackData = new MAG_TRACK_DATA2();

            TrackData.byTrackFlag = 0;
            if (checkBoxT1.Checked)
                TrackData.byTrackFlag |= 1;
            if (checkBoxT2.Checked)
                TrackData.byTrackFlag |= 2;
            if (checkBoxT3.Checked)
                TrackData.byTrackFlag |= 4;

            if (comboBoxCoercivity.SelectedIndex == 0)
                TrackData.byCoercivity = 0;
            else
                TrackData.byCoercivity = 1;

            if (checkBoxT1.Checked)
                TrackData.szTrack1 = Encoding.ASCII.GetBytes(textBoxT1.Text);
            if (checkBoxT2.Checked)
                TrackData.szTrack2 = Encoding.ASCII.GetBytes(textBoxT2.Text);
            if (checkBoxT3.Checked)
                TrackData.szTrack3 = Encoding.ASCII.GetBytes(textBoxT3.Text);

            dwRet = PavoApi.PAVO_WriteMagTrackData(printer, 0, TrackData);

            msg = String.Format("dwRet = {0}", dwRet);
            MessageBox.Show(this, msg, "PAVO_WriteMagTrackData");
        }

        private void buttonCheckPrinterStatus_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            uint dwStatus = 0;
            string msg;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            dwRet = PavoApi.PAVO_CheckPrinterStatus(printer, ref dwStatus);

            msg = String.Format("Printer Status = 0x{0:X8}", dwStatus);
            MessageBox.Show(this, msg, "CheckPrinterStatus");
        }

        private void buttonGetRFIDCardSN_Click(object sender, EventArgs e)
        {
            int nRet = 0;
            bool bSucc = false;

            byte[] cardSN = new byte[32];
            UTF8Encoding enc = new UTF8Encoding();
            string tempString = null;

            byte chChipID = 0;


            if (comboBoxCOMport.SelectedIndex < 1)
                return;

            nRet = E680API.E680_Open_ComPort(comboBoxCOMport.SelectedIndex + 1);
            if (nRet == 0)//fail to open COM port
                return;

            //try ISO14443A -----------------------------
            nRet = E680API.E680_Set_Readcard_Mode(0);//ISO14443A
            if (nRet == 1)
            {
                nRet = E680API.E680_Request_CardSN(cardSN);
                if (nRet == 1)
                {
                    tempString = enc.GetString(cardSN);
                    textBoxCardSN.Text = tempString;
                    bSucc = true;
                }
            }

            //try ISO14443B -----------------------------
            if (nRet == 0)
            {
                nRet = E680API.E680_Set_Readcard_Mode(1);//ISO14443B
                if (nRet == 1)
                {
                    nRet = E680API.E680_SR_initiate(ref chChipID);
                }

                if (nRet == 1)
                {
                    nRet = E680API.E680_SR_select(chChipID);
                }

                if (nRet == 1)
                {
                    nRet = E680API.E680_SR_get_uid(cardSN);
                    if (nRet == 1)
                    {
                        tempString = enc.GetString(cardSN);
                        textBoxCardSN.Text = tempString;
                        bSucc = true;
                    }
                }
            }

            //try ISO15693 ------------------------------
            if (nRet == 0)
            {
                nRet = E680API.E680_Set_Readcard_Mode(2);//ISO15693
                if (nRet == 1)
                {
                    nRet = E680API.E680_15693_Inventory(cardSN);
                    if (nRet == 1)
                    {
                        tempString = enc.GetString(cardSN);
                        textBoxCardSN.Text = tempString;
                        bSucc = true;
                    }
                }
            }

            nRet = E680API.E680_Close_ComPort();
        }

        private void buttonSetPassword_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            string msg;
            string szCurPasswd = null, szNewPasswd = null;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            SetPasswordDialog passwdDlg = new SetPasswordDialog();

            DialogResult dialogResult = passwdDlg.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                szCurPasswd = passwdDlg.strCurrentPasswd;
                szNewPasswd = passwdDlg.strNewPasswd;

                dwRet = PavoApi.PAVO_SetPassword(printer, szCurPasswd, szNewPasswd);

                msg = String.Format("dwRet = {0}", dwRet);
                MessageBox.Show(this, msg, "PAVO_SetPassword");
            }
        }

        private void buttonSetSecurityMode_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            string msg;
            string szCurPasswd = null;
            int nSecurityMode = 0;

            string printer;
            if (radioButton1.Checked)
                printer = comboBoxPrinterQueue.Text;
            else if (radioButton2.Checked)
                printer = textBoxIPaddress.Text;
            else
                printer = textBoxUSBLink.Text;

            SetSecurityModeDialog securityModeDlg = new SetSecurityModeDialog();

            DialogResult dialogResult = securityModeDlg.ShowDialog(this);
            if (dialogResult == DialogResult.OK)
            {
                szCurPasswd = securityModeDlg.strCurrentPasswd;
                nSecurityMode = securityModeDlg.nLockMode;

                dwRet = PavoApi.PAVO_SetSecurityMode(printer, szCurPasswd, nSecurityMode);

                msg = String.Format("dwRet = {0}", dwRet);
                MessageBox.Show(this, msg, "PAVO_SetSecurityMode");
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void EraseCard_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;
            
            string printer;
            string msg;

            if (true)//radioButton1.Checked)
            {
                //printer = comboBoxPrinterQueue.Text;
                if (radioButton1.Checked)
                    printer = comboBoxPrinterQueue.Text;
                else if (radioButton2.Checked)
                    printer = textBoxIPaddress.Text;
                else
                    printer = textBoxUSBLink.Text;
                dwRet = PavoApi.PAVO_EraseCardArea(printer, 10, 10, 400, 400);
                msg = String.Format("dwRet = {0}", dwRet);
            }
            else
            {
                msg = String.Format("Card Erased only supported by Printer queue!");
            }           
            MessageBox.Show(this, msg, "PAVO_EraseCardArea");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            uint dwRet = 0;

            string printer;
            string msg;

            if (true)//radioButton1.Checked)
            {
                //printer = comboBoxPrinterQueue.Text;
                if (radioButton1.Checked)
                    printer = comboBoxPrinterQueue.Text;
                else if (radioButton2.Checked)
                    printer = textBoxIPaddress.Text;
                else
                    printer = textBoxUSBLink.Text;

                byte bPos = (byte)Int32.Parse(textBoxPos.Text); //10; // 0<= bPos <=10
                byte bTime = (byte)Int32.Parse(textBoxTime.Text);// 10; // 0<= bTime <=255; 0 means forever

                dwRet = PavoApi.PAVO_SetStandbyParameters(printer, bPos, bTime);
                msg = String.Format("dwRet = {0}", dwRet);
            }
            else
            {
                msg = String.Format("Set standby parameters only supported by Printer queue!");
            }
            MessageBox.Show(this, msg, "PAVO_SetStandbyParameters");
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox10_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox15_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox13_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (client.IsOpen())
            {
                client.Close();
            }
            else
            {
                var url = String.Format(@"ws://{0}:57976/win", textBoxIPaddress.Text.Replace("\r\n", ""));
                client.Connect(url);
            }
        }

        /// SocketClient.IListener implement
        public void OnStateChanged(WebSocketState state)
        {
            labelSocketMessage.Text = state.ToString();
        }

        public void OnMessage(string text)
        {
            try
            {
                labelSocketMessage.Text = text;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void OnMessage(MemoryStream ms) {
            try
            {
                pictureBoxCard.Image = new Bitmap(ms);
                var socketBitmap = new Bitmap(ms);
                socketBitmap.Save(filePath, ImageFormat.Bmp);
                buttonSocketPrint.PerformClick();
                //ImageCodecInfo encoder = GetEncoder(ImageFormat.Bmp);
                //System.Drawing.Imaging.Encoder QualityEncoder = System.Drawing.Imaging.Encoder.Quality;
                //EncoderParameters myEncoderParameters = new EncoderParameters(1);
                //EncoderParameter myEncoderParameter = new EncoderParameter(QualityEncoder, 20L);
                //myEncoderParameters.Param[0] = myEncoderParameter;
                //socketBitmap.Save(@"C:\Huy\img.bmp", encoder, myEncoderParameters);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void OnError(Exception e)
        {
            MessageBox.Show(e.Message);
        }

        private void pictureBoxCard_Click(object sender, EventArgs e)
        {

        }

        private void buttonSocketPrint_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == false)
            {
                MessageBox.Show("Please select printer queue.", "Print By Driver", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            PAVO_JOB_PROPERTY JobProp = new PAVO_JOB_PROPERTY();

            m_nPageToPrint = 0;
            m_nPagePrinted = 0;

            JobProp.dwFieldFlag = PavoApi.FF_CARD_TYPE | PavoApi.FF_FLAGS | PavoApi.FF_PARENT_HWND | PavoApi.FF_ORIENTATION
                     | PavoApi.FF_COPIES | PavoApi.FF_DUPLEX | PavoApi.FF_RIBBON_TYPE | PavoApi.FF_PRINT_COLOR;

            JobProp.hParentWnd = this.Handle;

            JobProp.shOrientation = (short)(comboBoxOrientation.SelectedIndex + 1);
            JobProp.shCopies = 1;

            switch (comboBoxRibbon.SelectedIndex)
            {
                case 0: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKO; break;
                case 1: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_K; break;
                case 2: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_HALF_YMCKO; break;
                case 3: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_YMCKOK; break;
                case 4: JobProp.byRibbonType = (byte)PavoApi.PAVO_RIBBON_TYPE_KO; break;
            }

            JobProp.dwCardType = (uint)comboBoxCardType.SelectedIndex;

            JobProp.dwFlags = 0;
            if (checkBox1.Checked)
                JobProp.dwFlags = PavoApi.PAVO_FLAG_NOT_SHOW_ERROR_MSG_DLG;
            if (checkBox2.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_WAIT_MSG_DONE;
            if (checkBox3.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_NOT_SHOW_CLEAN_MSG;
            if (checkBox4.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_WATCH_JOB_PRINTED;
            if (checkBox6.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_NOT_EJECT_CARD_AFTER_PRINTED;
            if (checkBox7.Checked)
                JobProp.dwFlags |= PavoApi.PAVO_FLAG_MOVE_CARD_TO_STANDBY_AFTER_PRINTED;

            if (checkBox5.Checked)
                JobProp.byPrintAs230e = 1;
            else
                JobProp.byPrintAs230e = 0;

            if (comboBoxFrontSide.SelectedIndex > 0)
            {
                JobProp.byDuplex = 1;
                m_nPageToPrint++;
            }
            if (comboBoxBackSide.SelectedIndex > 0)
            {
                JobProp.byDuplex |= 2;
                m_nPageToPrint++;
            }

            if (comboBoxFrontSide.SelectedIndex == 1 || comboBoxFrontSide.SelectedIndex == 3)
                JobProp.byPrintColor = 1;
            if (comboBoxBackSide.SelectedIndex == 1 || comboBoxBackSide.SelectedIndex == 3)
                JobProp.byPrintColor |= 2;

            if (checkBoxK1.Checked && (comboBoxFrontSide.SelectedIndex == 2 || comboBoxFrontSide.SelectedIndex == 3))
            {
                JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
                JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_FRONT;
            }
            if (checkBoxK2.Checked && (comboBoxBackSide.SelectedIndex == 2 || comboBoxBackSide.SelectedIndex == 3))
            {
                JobProp.dwFieldFlag |= PavoApi.FF_DATA_FLAG;
                JobProp.dwDataFlag |= PavoApi.PAVO_DATAFLAG_RESIN_BACK;
            }

            //create print job
            PrintDocument pd = new PrintDocument();

            pd.PrinterSettings.PrinterName = comboBoxPrinterQueue.Text;
            pd.DefaultPageSettings.Margins.Left = 0;
            pd.DefaultPageSettings.Margins.Top = 0;
            pd.DefaultPageSettings.Margins.Right = 0;
            pd.DefaultPageSettings.Margins.Bottom = 0;
            pd.DocumentName = "DemoCSharp Print Job";

            PavoApi.PAVO_ApplyJobSetting(pd.PrinterSettings, ref JobProp);

            pd.PrintPage += new PrintPageEventHandler(OnSocketPrintPage);

            try
            {
                pd.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "列印失敗", MessageBoxButtons.OK, MessageBoxIcon.Error);
                pd.PrintController.OnEndPrint(pd, new PrintEventArgs());
            }

            pd.PrintPage -= OnSocketPrintPage;
        }

        
       
        private void OnSocketPrintPage(object sender, PrintPageEventArgs e)
        {
            Image colorImg = null;
            Rectangle rect;

            uint dwRet = 0;
            IntPtr hPrinterDC;
            uint dwDataTypeK = 0;
            Bitmap bmpK8;

            if (comboBoxOrientation.SelectedIndex == 0)//portrait
                rect = new Rectangle(0, 0, 642, 1014);// origin 642 x 1014
            else//landscape
                rect = new Rectangle(0, 0, 1014, 642);

            e.Graphics.PageUnit = GraphicsUnit.Pixel;

            //---------------------------------------------
            hPrinterDC = e.Graphics.GetHdc();

            //send resin K ----------------------
            if (m_nPageToPrint == 1)
            {
                if (comboBoxFrontSide.SelectedIndex > 0 && checkBoxK1.Checked)
                    dwDataTypeK = PavoApi.PAVO_DATA_RESIN_FRONT;
                else if (comboBoxBackSide.SelectedIndex > 0 && checkBoxK2.Checked)
                    dwDataTypeK = PavoApi.PAVO_DATA_RESIN_BACK;
            }
            else if (m_nPageToPrint == 2)
            {
                if (m_nPagePrinted == 0)//front side
                {
                    if (checkBoxK1.Checked)
                        dwDataTypeK = PavoApi.PAVO_DATA_RESIN_FRONT;
                }
                else//back side
                {
                    if (checkBoxK2.Checked)
                        dwDataTypeK = PavoApi.PAVO_DATA_RESIN_BACK;
                }
            }

            if (dwDataTypeK != 0)
            {
                if (dwDataTypeK == PavoApi.PAVO_DATA_RESIN_FRONT)
                    bmpK8 = (Bitmap)Bitmap.FromFile(filePath);
                else
                    bmpK8 = (Bitmap)Bitmap.FromFile(filePath);
                dwRet = PavoApi.PAVO_SetExtraDataToHDC(hPrinterDC, dwDataTypeK, bmpK8);
            }

            e.Graphics.ReleaseHdc(hPrinterDC);
            //---------------------------------------------

            //send color image ============================
            if (m_nPageToPrint == 1)
            {
                if (checkBoxColor1.Checked)
                {
                    //InsertText("Front Side");
                    colorImg = Image.FromFile(filePath);
                }
                else if (checkBoxColor2.Checked)
                {
                    //InsertText("Back Side");
                    colorImg = Image.FromFile(filePath);
                }

                if (colorImg != null)
                    e.Graphics.DrawImage(colorImg, rect);

                // Create string to draw.
                String drawString = "";

                // Create font and brush.
                Font drawFont = new Font("Arial", 16);
                SolidBrush drawBrush = new SolidBrush(Color.Black);

                // Create point for upper-left corner of drawing.
                PointF drawPoint = new PointF(150.0F, 150.0F);

                // Draw string to screen.
                e.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
            }
            else if (m_nPageToPrint == 2)
            {
                if (m_nPagePrinted == 0)
                {
                    if (checkBoxColor1.Checked)
                    {
                        //InsertText("Front Side");
                        colorImg = Image.FromFile(filePath);
                    }
                }
                else
                {
                    if (checkBoxColor2.Checked)
                    {
                        //InsertText("Back Side");
                        colorImg = Image.FromFile(filePath);
                    }
                }

                if (colorImg != null)
                    e.Graphics.DrawImage(colorImg, rect);
            }

            m_nPagePrinted++;

            if (m_nPageToPrint == m_nPagePrinted)
                e.HasMorePages = false;
            else
                e.HasMorePages = true;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        private void textBoxIPaddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}