using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintService
{
    public partial class MainForm : Form
    {
        private PrintRepository printRepository = new PrintRepository();
        private XMLRepository XMLRepository = new XMLRepository();

        public MainForm()
        {
            InitializeComponent();
        }

        //Get the company info from to the XML file
        public void GetData()
        {
            //Header
            printRepository.OrganizationName = XMLRepository.ReadNode("/Configuration/organizationName");
            printRepository.LegalPerson = XMLRepository.ReadNode("/Configuration/LegalPerson");
            printRepository.IDLegalPerson = XMLRepository.ReadNode("/Configuration/IDLegalPerson");
            printRepository.PhoneNumber = XMLRepository.ReadNode("/Configuration/PhoneNumber");
            printRepository.Address = XMLRepository.ReadNode("/Configuration/Address");

            //SellInfo
            /*
             * In this part the system extract the info from DB
             */
            printRepository.Vendor = "Sistema";
            printRepository.InvoiceNumber = "1";
            printRepository.Client = "Sergio 06/11/17";

            //Buttom
            printRepository.LegalInformation = XMLRepository.ReadNode("/Configuration/legalInformation");
            printRepository.Slogan = XMLRepository.ReadNode("/Configuration/Slogan");
            printRepository.Sendoff = XMLRepository.ReadNode("/Configuration/Sendoff");
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            this.DGVProducts.Rows.Add(txtName.Text, txtPrice.Text);

            txtName.Text = "";
            txtPrice.Text = "";
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in DGVProducts.SelectedRows)
            {
                DGVProducts.Rows.RemoveAt(row.Index);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            //initiazate data
            printRepository.ClearData();
            printRepository.Cash = txtCash.Text;
            printRepository.MyFont = new Font("Courier New", 12);
            printRepository.Style = new SolidBrush(Color.Black);
            printRepository.fontHeight = printRepository.MyFont.GetHeight();
            printRepository.DGVProducts = this.DGVProducts;

            PrintDialog printDialog = new PrintDialog();
            PrintDocument printDocument = new PrintDocument();
            printDialog.Document = printDocument; //add the document to the dialog box...  
            printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(CreateReceipt); //add an event handler that will do the printing
            printDocument.Print();
        }

        public void CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            printRepository.graphic = graphic;

            printRepository.PrintHeader();

            printRepository.PrintSellInfo();

            printRepository.PrintProductList();

            printRepository.PrintTotals();

            printRepository.PrintButtom();

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            GetData();
        }
    }
}
