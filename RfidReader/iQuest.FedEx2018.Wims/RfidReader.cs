using iQuest.FedEx2018.Wims.Properties;
using RRU4DotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace iQuest.FedEx2018.Wims
{
    internal class RfidReader : IDisposable
    {
        private const string endInventoryFileName = "endInventory.txt";

        private readonly CRRU4 reader;
        private bool disposed = false;
        private readonly HashSet<string> tagsRead = new HashSet<string>();

        internal RfidReader()
        {
            reader = new CRRU4();
            reader.SetResultHandlerASyncGetEPCs(ResultHandlerASyncGetEPCsSync,
                ResultHandlerASyncGetEPCsASync);

            var comConfigData = new tCommunicationConfigData
            {
                enCommunicationType = tCommunicationType.CT_Ethernet
            };
            comConfigData.stEthernet.bIsIPAddress = false;
            comConfigData.stEthernet.enIPAddressType = tIPAddressType.IPAT_IPv4;
            comConfigData.stEthernet.sNetworkName = "192.168.0.1";

            if (reader.ConnectReader(comConfigData) == tReaderErrorCode.REC_NoError)
            {
                Console.WriteLine("Connected");
            }
            else
            {
                Console.WriteLine("Error connecting");
            }
        }

        internal void ResultHandlerASyncGetEPCsSync(CRRU4 pRRU4, tResultFlag enResultFlag)
        {
            Console.WriteLine(string.Format("ResultHandlerASyncGetEPCsSync called, enResultFlag = {0}",
                enResultFlag));
        }

        internal void ResultHandlerASyncGetEPCsASync(CRRU4 pRRU4, 
            tComingGoingFlag enComingGoingFlag, 
            tExtendedResultFlagMask enExtendedResultFlag,
            tEPCListEntry[] rgEPCList)
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("Tag coming/going = {0}, tags received:",
                enComingGoingFlag));
            foreach (tEPCListEntry epcListEnry in rgEPCList)
            {
                Console.WriteLine(epcListEnry.pEPC);
                if (epcListEnry.pEPC.ToString() != string.Empty)
                {
                    tagsRead.Add(epcListEnry.pEPC.ToString());
                }
            }
            Console.WriteLine();
        }

        internal void StartReading()
        {
            if (reader.ASyncGetEPCs() == tReaderErrorCode.REC_NoError)
            {
                Console.WriteLine("Read started!");
            }
        }

        internal void Save()
        {
            string fileName = string.Format("Tags {0}.txt", DateTime.Now.ToString("dd-mm-yyyy hh_mm_ss ffff"));
            string filePath = GetFilePath(fileName);

            try
            {
                File.WriteAllLines(filePath,
                    new List<string>(tagsRead).ToArray());
                Console.WriteLine(string.Format("Data saved to path '{0}'", filePath));
                tagsRead.Clear();
            }
            catch (Exception exc)
            {
                Console.WriteLine(string.Format("Exception during saving to {0}: {1}",
                    fileName,
                    exc.Message));
            }
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(Settings.Default.TagsReportFolder, fileName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing && reader != null)
            {
                reader.DisconnectReader();
                string endInventoryFilePath = GetFilePath(endInventoryFileName);

                try
                {
                    File.WriteAllText(endInventoryFilePath, "");
                    Console.WriteLine(string.Format("Endinvontory signaled with '{0}'", endInventoryFilePath));
                }
                catch (Exception exc)
                {
                    Console.WriteLine(string.Format("Exception during creating end inventory file: {0}",
                        exc.Message));
                }
            }

            disposed = true;
        }
    }
}
