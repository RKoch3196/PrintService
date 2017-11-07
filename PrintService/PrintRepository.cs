using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintService
{
    class PrintRepository
    {

        #region InvoiceConfigData
        public Graphics graphic { get; set; }
        public DataGridView DGVProducts { get; set; }
        public Font MyFont { get; set; }
        public SolidBrush Style { get; set; }
        public float fontHeight { get; set; }
        public int startX = 5;
        public int startY = 0;
        public int offset = 30;
        public float TotalToPay = 0.00f;
        public float Change = 0.00f;
        #endregion

        #region HeaderInfo
        public string OrganizationName { get; set; }
        public string LegalPerson { get; set; }
        public string IDLegalPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        #endregion

        #region SellInfo
        public string Vendor { get; set; }
        public string InvoiceNumber { get; set; }
        public string Cash { get; set; }
        public string Client { get; set; }
        #endregion

        #region ButtomInfo
        public string LegalInformation { get; set; }
        public string Slogan { get; set; }
        public string Sendoff { get; set; }
        #endregion

        #region private Methods
        private float ConvertCash(string cashText)
        {
            return float.Parse(cashText.Substring(1, cashText.Length - 1));
        }

        private int CalcPaddingValue(string NameSize, string PrizeSize)
        {
            return 32 - (NameSize.Length + PrizeSize.Length);
        }

        private int CalcPaddingValue(string Text)
        {
            return 32 - Text.Length;
        }

        private string AddSpace(int NumSpace, char delimiter)
        {
            string final = string.Empty;
            for (int i = 0; i < NumSpace; i++)
            {
                final += delimiter;
            }
            return final;
        }

        private void AddSeparator()
        {
            string final = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                final += "- ";
            }

            graphic.DrawString(final, MyFont, Style, startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent
        }

        private void PrintInCenter(string[] Variables)
        {
            string FinalText = string.Empty;
            int PaddingValue = 0;
            for (int i = 0; i < Variables.Length; i++)
            {
                string Current = Variables[i];
                PaddingValue = CalcPaddingValue(Current);
                FinalText += AddSpace(PaddingValue / 2, ' ') + Current + AddSpace(PaddingValue, ' ') + Environment.NewLine;

            }
            graphic.DrawString(FinalText, MyFont, Style, startX, startY);
            offset = offset + (int)fontHeight + 20; //make the spacing consistent
        }

        private void PrintInCenter(string[] Variables, int state)
        {
            string FinalText = string.Empty;
            int PaddingValue = 0;
            for (int i = 0; i < Variables.Length; i++)
            {
                string Current = Variables[i];
                PaddingValue = CalcPaddingValue(Current);
                FinalText += AddSpace(PaddingValue / 2, ' ') + Current + AddSpace(PaddingValue, ' ') + Environment.NewLine;

            }
            graphic.DrawString(FinalText, MyFont, Style, startX, startY + offset);
            offset = offset + (int)fontHeight + 20; //make the spacing consistent
            graphic.DrawString(" ", MyFont, Style, startX, startY + offset);
        }

        private void PrintRight(string[] Variables)
        {
            string FinalText = string.Empty;
            int PaddingValue = 0;
            for (int i = 0; i < Variables.Length; i = i + 2)
            {
                string CurrentName = Variables[i];
                string CurrentValue = "";
                if (i <= Variables.Length)
                {
                    CurrentValue = Variables[i + 1];
                }
                PaddingValue = CalcPaddingValue(CurrentName, CurrentValue);
                FinalText += CurrentName + AddSpace(PaddingValue, '.') + CurrentValue + Environment.NewLine;

            }
            graphic.DrawString(FinalText, MyFont, Style, startX, startY + offset);
            offset = offset + ((int)fontHeight + 30); //make the spacing consistent
        }

        #endregion

        #region Public Methods
        public void ClearData()
        {
            startX = 5;
            startY = 5;
            offset = 40;
            TotalToPay = 0;
        }

        public void PrintHeader()
        {
            string[] Variables = { OrganizationName, LegalPerson, IDLegalPerson, PhoneNumber };

            PrintInCenter(Variables);

            graphic.DrawString(Address, MyFont, Style, 0, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent
        }

        public void PrintSellInfo()
        {
            AddSeparator();

            string[] Variables = { "Fact #", InvoiceNumber, "Vendedor: ", Vendor, "Cliente: ", Client };
            PrintRight(Variables);
        }

        public void PrintProductList()
        {
            AddSeparator();

            string ProductText = string.Empty;
            int PaddingValue = CalcPaddingValue("Producto", "Precio");
            string top = "Producto" + AddSpace(PaddingValue, ' ') + "Precio";
            graphic.DrawString(top, MyFont, Style, startX, startY + offset);
            offset = offset + (int)fontHeight + 5; //make the spacing consistent

            AddSeparator();

            foreach (DataGridViewRow row in DGVProducts.Rows)
            {
                string ProductName = row.Cells[0].Value.ToString();
                string Price = row.Cells[1].Value.ToString();

                PaddingValue = CalcPaddingValue(ProductName, Price);

                ProductText += ProductName + AddSpace(PaddingValue, '.') + Price + Environment.NewLine;

                TotalToPay = TotalToPay + float.Parse(row.Cells[1].Value.ToString());
            }
            graphic.DrawString(ProductText, MyFont, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + ((int)fontHeight + 5) * DGVProducts.RowCount / 2; //make the spacing consistent

            Change = (ConvertCash(Cash) - TotalToPay);

            AddSeparator();
        }

        public void PrintTotals()
        {
            string[] Totals = { "Total a pagar:", TotalToPay + "", "Paga con:", Cash + "", "Su cambio:", Change + "" };
            PrintRight(Totals);

            AddSeparator();
        }

        public void PrintButtom()
        {
            string[] Variables = { LegalInformation, "Codigo 552004", Slogan, Sendoff };
            PrintInCenter(Variables, 1);
        }
        #endregion
    }
}
