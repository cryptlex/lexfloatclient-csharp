using System;
using System.Windows.Forms;
using Cryptlex;

namespace FloatSample
{
    public partial class Form1 : Form
    {
        LexFloatClient floatClient;
        public Form1()
        {
            InitializeComponent();
            int status;
            status = LexFloatClient.SetProductFile("Product.dat");
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error setting product file: " + status.ToString();
                return;
            }
        }

        private void LicenceRefreshCallback(uint status)
        {
            switch (status)
            {
                case LexFloatClient.LF_E_LICENSE_EXPIRED:
                    this.statusLabel.Text = "The lease expired before it could be renewed.";
                    break;
                case LexFloatClient.LF_E_LICENSE_EXPIRED_INET:
                    this.statusLabel.Text = "The lease expired due to network connection failure.";
                    break;
                case LexFloatClient.LF_E_SERVER_TIME:
                    this.statusLabel.Text = "The lease expired because Server System time was modified.";
                    break;
                case LexFloatClient.LF_E_TIME:
                    this.statusLabel.Text = "The lease expired because Client System time was modified.";
                    break;
                default:
                    this.statusLabel.Text = "The lease expired due to some other reason.";
                    break;
            }
        }

        private void leaseBtn_Click(object sender, EventArgs e)
        {
            if (floatClient != null && floatClient.HasLicense() == LexFloatClient.LF_OK)
            {
                return;
            }
            int status;
            floatClient = new LexFloatClient();
            status = floatClient.SetVersionGUID("59A44CE9-5415-8CF3-BD54-EA73A64E9A1B");
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error setting version GUID: " + status.ToString();
                return;
            }
            status = floatClient.SetFloatServer( "localhost", 8090);
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error Setting Host Address: " + status.ToString();
                return;
            }
            status = floatClient.SetLicenseCallback( LicenceRefreshCallback);
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error Setting Callback Function: " + status.ToString();
                return;
            }
            status = floatClient.RequestLicense();
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error Requesting License: " + status.ToString();
                return;
            }
            this.statusLabel.Text = "License leased successfully!";
        }

        private void dropBtn_Click(object sender, EventArgs e)
        {
            if(floatClient == null)
            {
                return;
            }
            int status;
            status = floatClient.DropLicense();
            if (status != LexFloatClient.LF_OK)
            {
                this.statusLabel.Text = "Error Dropping License: " + status.ToString();
                return;
            }
            this.statusLabel.Text = "License dropped successfully!";
            
        }
    }
}
