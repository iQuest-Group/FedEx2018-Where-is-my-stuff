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
            reader.SetResultHandlerSyncGetEPCs(DelegateResultHandlerSyncGetEPCs);
            reader.SetResultHandlerASyncReadDataUntilEndOfBankAny(
                DelegateResultHandlerASyncReadDataUntilEndOfBankAnySync,
                DelegateResultHandlerASyncReadDataUntilEndOfBankAnyASync);
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

        internal void DelegateResultHandlerSyncGetEPCs(CRRU4 pRRU4, tResultFlag enResultFlag, tExtendedResultFlagMask enExtendedResultFlag, tEPCListEntry[] rgEPCList)
        {
            // Method intentionally left empty.
        }

        internal void DelegateResultHandlerASyncReadDataUntilEndOfBankAnySync(CRRU4 pRRU4, tResultFlag enResultFlag)
        {
        }

        internal void DelegateResultHandlerASyncReadDataUntilEndOfBankAnyASync(CRRU4 pRRU4, tComingGoingFlag enComingGoingFlag, tExtendedResultFlagMask enExtendedResultFlag, tEPCListEntry[] rgEPCList)
        { }

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
