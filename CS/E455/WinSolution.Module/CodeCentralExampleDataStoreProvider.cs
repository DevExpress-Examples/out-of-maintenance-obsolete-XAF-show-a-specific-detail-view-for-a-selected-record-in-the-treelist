using System;
using System.Data;
using DevExpress.Xpo.DB;

namespace WinSolution.Module {
    //For demo purposes only!!!
    public class CodeCentralExampleDataStoreProvider {
        private static readonly string fConnectionString;
        private static readonly DataSet fdataSet;
        public static string ConnectionString { get { return fConnectionString; } }
        static CodeCentralExampleDataStoreProvider() {
            string providerKey = Guid.NewGuid().ToString();
            fConnectionString = "XpoProvider=" + providerKey;
            fdataSet = new DataSet();
            DataStoreBase.RegisterDataStoreProvider(providerKey, CreateProviderFromString);
        }
        public static IDataStore CreateProviderFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            objectsToDisposeOnDisconnect = new IDisposable[] { };
            return new DataSetDataStore(fdataSet, autoCreateOption);
        }
    }
}