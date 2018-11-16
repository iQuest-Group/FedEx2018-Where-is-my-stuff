using RRU4DotNet;
using System;

namespace iQuest.FedEx2018.Wims
{
    internal class RfidReader : IDisposable
    {
        private readonly CRRU4 reader;
        bool disposed = false;

        internal RfidReader()
        {
            reader = new CRRU4();
            //reader.SetResultHandlerSyncGetEPCs(DelegateResultHandlerSyncGetEPCs);
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
                Console.WriteLine("Erro connecting");
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
            }

            disposed = true;
        }
    }

}
