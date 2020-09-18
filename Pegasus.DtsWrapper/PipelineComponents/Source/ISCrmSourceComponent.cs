using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime;

namespace Pegasus.DtsWrapper.Source
{
    public class ISCrmSourceComponent : ISSourceComponent
    {

        #region ctor

        internal ISCrmSourceComponent(IDTSComponentMetaData100 component) : base(component)
        {
            InitDefaults();
        }

        public ISCrmSourceComponent(ISDataFlowTask parentDataFlowTask, string componentname) :
            base(parentDataFlowTask, "KingswaySoft.IntegrationToolkit.DynamicsCrm.CrmSourceComponent", componentname)
        {
            InitDefaults();
            
        }

        public ISCrmSourceComponent(ISDataFlowTask parentDataFlowTask, string componentname, ISConnectionManager cm, bool connectionIsOffline = false) :
            this(parentDataFlowTask, componentname)
        {
            if (!(connectionIsOffline))
            {
                if (cm != null)
                    AssignConnectionManager(cm);
                else
                    System.Console.WriteLine("WARN::: No Connection Manager is attached. Please use the AssignConnectionManager method to assign a Connection Manager.");
            }
        }

        #region default inits

        private void InitDefaults()
        {
            _numberOfOutputsAllowed = 1;
            _numberOfInputsAllowed = 0;
        }

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Specifies the type of source data from Microsoft Dynamics CRM.
        /// </summary>
        public int SourceType
        {
            get { return CustomPropertyGetter<int>("SourceType"); }
            set { CustomPropertySetter<int>("SourceType", value); }
        }
        /// <summary>
        /// Dynamics CRM entity to retrieve data from.
        /// </summary>
        public string SourceEntity
        {
            get { return CustomPropertyGetter<string>("SourceEntity"); }
            set { CustomPropertySetter<string>("SourceEntity", value); }
        }
        /// <summary>
        /// FetchXML statement.
        /// </summary>
        public string FetchXML
        {
            get { return CustomPropertyGetter<string>("FetchXML"); }
            set { CustomPropertySetter<string>("FetchXML", value); }
        }
        /// <summary>
        /// Specifies the batch size of the query.
        /// </summary>
        public int BatchSize
        {
            get { return CustomPropertyGetter<int>("BatchSize"); }
            set { CustomPropertySetter<int>("BatchSize", value); }
        }

        /// <summary>
        /// Specify the maximum number of rows to be returned from the Dynamics 365/CRM server (Specify 0 to read all satisfying records).
        /// </summary>
        public int MaxRowsReturned
        {
            get { return CustomPropertyGetter<int>("MaxRowsReturned"); }
            set { CustomPropertySetter<int>("MaxRowsReturned", value); }
        }

        /// <summary>
        /// The caller to be used for CRM impersonation when reading from CRM server.
        /// </summary>
        public string ImpersonateAs
        {
            get { return CustomPropertyGetter<string>("ImpersonateAs"); }
            set { CustomPropertySetter<string>("ImpersonateAs", value); }
        }
        /// <summary>
        /// Specifies the output timezone for CRM datetime fields.
        /// </summary>
        public int OutputTimezone
        {
            get { return CustomPropertyGetter<int>("OutputTimezone"); }
            set { CustomPropertySetter<int>("OutputTimezone", value); }
        }
        #endregion

        #region Assign Connection Manager

        private void AssignConnectionManager(ConnectionManager cmg)
        {
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(cmg);
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManagerID = cmg.ID;
            _connectionAssgined = true;
        }

        public void AssignConnectionManager(ISConnectionManager cm)
        {
            AssignConnectionManager(cm.ConnectionManager);
        }

        #endregion
    }
}
