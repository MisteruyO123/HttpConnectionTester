using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Threading;
using System.ComponentModel;

namespace HttpConnectionTester
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        string m_strResult = "";
        string[] m_arrURL = new string[22];
        int m_index = 0;
        BackgroundWorker worker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            InitiateURLValues();           
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ExecuteURLConnectionTest(sender);            
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pbTest.Value = e.ProgressPercentage;
            tbURL.Text = m_arrURL[m_index];
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.IsEnabled = true;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //Thread t_Test = new Thread(ExecuteURLConnectionTest);
            //t_Test.Start();
            btnStart.IsEnabled = false;
            worker.RunWorkerAsync();               
        }
        
        /// <summary>
        /// git hub test
        /// </summary>
        private void InitiateURLValues()
        {
            m_arrURL[0] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_GetCOAInformation/bpel_syds_getcoa_client_ep";
            m_arrURL[1] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_ChecktoCOAAAI/bpel_syds_checktocoaaai_client_ep";
            m_arrURL[2] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_getMasterOrderNo/bpel_syds_getmasterorderno_client_ep";
            m_arrURL[3] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_AR/bpel_syds_ar_client_ep";
            m_arrURL[4] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_ClosingCheckwithCOandDGJ/bpel_syds_closingcheckwithcodgj_client_ep";
            m_arrURL[5] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_GA/bpel_syds_ga_client_ep";
            m_arrURL[6] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_AP/bpel_syds_ap_client_ep";
            m_arrURL[7] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_getTrustCOA/bpel_syds_gettrustcoa_client_ep";
            m_arrURL[8] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_FindDueDateDiscountDateforAPAR/bpel_findduedateforapar_client_ep";
            m_arrURL[9] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_JEPerFyCtry/bpel_syds_jeperfyctry_client_ep";
            m_arrURL[10] = "http://130.1.24.243/soa-infra/services/m90_syds/SYDS_ERP_RetrieveAccountFromAAI/bpel_syds_retrieveaccountaai_client_ep";

            m_arrURL[11] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_GA/bpel_entecga_client_ep";
            m_arrURL[12] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_getMasterOrderNo/bpel_entec_getmasterorderno_client_ep";
            m_arrURL[13] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_FindDueDateDiscountDateforAPAR/bpel_entec_findduedatediscountdateforapar_client_ep";
            m_arrURL[14] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_ClosingCheckwithCOandDGJ/bpel_entec_closingcheckwithcoanddgj_client_ep";
            m_arrURL[15] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_ChecktoCOAAAI/bpel_entec_checktocoaaai_client_ep";
            m_arrURL[16] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_GetCOAInformation/bpel_entec_getcoainformation_client_ep";
            m_arrURL[17] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_JEPerFyCtry/bpel_entec_jeperfyctry_client_ep";
            m_arrURL[18] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_RetrieveAccountFromAAI/bpel_entec_retrieveaccountfromaai_client_ep";
            m_arrURL[19] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_AP/bpel_entec_approcess_client_ep";
            m_arrURL[20] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_AR/bpel_entecarprocess_client_ep";
            m_arrURL[21] = "http://130.1.24.243:80/soa-infra/services/m90_entech/Entec_ERP_GA/bpel_entecga_client_ep";
        }

        private void ExecuteURLConnectionTest(object sender)
        {
            try
            {
                HttpWebRequest reqFP;
                HttpWebResponse rspFP;

                for (int i = 0; i < m_arrURL.Length; i++)
                {
                    m_index = i;
                    reqFP = (HttpWebRequest)HttpWebRequest.Create(m_arrURL[i]);
                    rspFP = (HttpWebResponse)reqFP.GetResponse();

                    if (HttpStatusCode.OK == rspFP.StatusCode)
                    {
                        // HTTP = 200 - Internet connection available, server online
                        rspFP.Close();
                    }
                    else
                    {
                        // Other status - Server or connection not available
                        rspFP.Close();
                        m_strResult += m_arrURL[i] + "\n";
                    }

                    (sender as BackgroundWorker).ReportProgress(i * 5);                  
                }

                if (m_strResult == "")
                    MessageBox.Show("URL Connection is Success !!");
                else
                    MessageBox.Show(m_strResult + "is Failed !!");
            }
            catch (WebException)
            {
                // Exception - connection not available
                MessageBox.Show("URL Connection is Failed !! can't get StatusCode");
            }

            (sender as BackgroundWorker).ReportProgress(0);
        }

    }
}
