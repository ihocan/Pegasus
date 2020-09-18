using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;


namespace Pegasus.DtsWrapper.Destination
{
    public class ISFlatFileDestination : ISDestinationComponent
    {
        #region ctor

        internal ISFlatFileDestination(IDTSComponentMetaData100 component) : base(component)
        {
            InitDefaults();
        }

        public ISFlatFileDestination(ISDataFlowTask parentDataFlowTask, string componentname) :
            base(parentDataFlowTask, "Microsoft.FlatFileDestination", componentname)
        {
            //InitDefaults();
            ExternalColumnInputColumnMap = new List<ExternalColumnInputMap>();

        }

        public ISFlatFileDestination(ISDataFlowTask parentDataFlowTask, string componentName,
            ISPipelineComponent sourceComponent,
            string sourceOutputName = "") :
            this(parentDataFlowTask, componentName)
        {
            if (String.IsNullOrEmpty(sourceOutputName))
                ConnectToAnotherPipelineComponent(sourceComponent.Name);
            else
                ConnectToAnotherPipelineComponent(sourceComponent.Name, sourceOutputName);

            InitDefaults();
        }

        #endregion

        #region default inits

        public void InitDefaults()
        {
            _numberOfOutputsAllowed = 0;
            _numberOfInputsAllowed = 1;
            Input = new ISInput(this, 0);
            ExternalColumnInputColumnMap = new List<ExternalColumnInputMap>();
        }

        #endregion

        #region Helper Properties

        public string DestinationPassword { get; set; }

        #endregion

        #region Dts Properties

        #region Header

        public string Header
        {
            get { return CustomPropertyGetter<string>("Header"); }
            set { CustomPropertySetter<string>("Header", value); }
        }

        #endregion

        #region Overwrite

        public bool Overwrite
        {
            get { return CustomPropertyGetter<bool>("Overwrite"); }
            set { CustomPropertySetter<bool>("Overwrite", value); }
        }

        #endregion

        #region CommandTimeOut

        public int CommandTimeout
        {
            get { return CustomPropertyGetter<int>("CommandTimeout"); }
            set { CustomPropertySetter<int>("CommandTimeout", value); }
        }

        #endregion

        #region Connection

        public ISConnectionManager ConnMgr
        {
            get;
            set;
        }

        private string _connection;
        public string Connection
        {
            get
            {
                //return ComponentMetaData.RuntimeConnectionCollection[0].Name;
                return _connection;
            }
            set
            {
                _connection = value;
                AssignConnectionManager(value);
            }
        }

        #endregion



        private void MapInputColumnsToExternalColumns()
        {
            if (ExternalColumnInputColumnMap.Count > 0)
            {
                string[] viCols = new string[Input.Input.GetVirtualInput().VirtualInputColumnCollection.Count];
                for (int i = 0; i < viCols.Length; i++)
                {
                    viCols.SetValue(Input.Input.GetVirtualInput().VirtualInputColumnCollection[i].Name, i);
                }

                foreach (ExternalColumnInputMap map in ExternalColumnInputColumnMap)
                {
                    for (int mc = 0; mc < DtsInput.ExternalMetadataColumnCollection.Count; mc++)
                    {
                        if (DtsInput.ExternalMetadataColumnCollection[mc].Name == map.ExternalColumn.ExternalColumnName)
                        {
                            //IDTSInputColumn100 ic = GetInputColumn(DtsInput.Name, map.Item2);
                            //ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
                            for (int vi = 0; vi < viCols.Length; vi++)
                            {
                                if (viCols[vi] == map.InputColumnName)
                                {
                                    ISInputColumn ic = new ISInputColumn(this, DtsInput.Name, viCols[vi], UsageType.UT_READONLY);
                                    //ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
                                    //MapInputColumn(DtsInput.Name, DtsInput.ExternalMetadataColumnCollection[mc].Name);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                string[] viCols = new string[Input.Input.GetVirtualInput().VirtualInputColumnCollection.Count];
                for (int i = 0; i < viCols.Length; i++)
                {
                    viCols.SetValue(Input.Input.GetVirtualInput().VirtualInputColumnCollection[i].Name, i);
                }
                for (int i = 0; i < viCols.Length; i++)
                {
                    for (int mc = 0; mc < DtsInput.ExternalMetadataColumnCollection.Count; mc++)
                    {
                        if (DtsInput.ExternalMetadataColumnCollection[mc].Name == viCols[i])
                        {
                            ISInputColumn ic = new ISInputColumn(this, DtsInput.Name, viCols[i], UsageType.UT_READONLY);
                            ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
                        }
                    }
                }
            }
        }

        private void PopulateMetadata()
        {
            bool retrieved = RetrieveMetaData();
        }

        #endregion

        #region Methods

        #region Assign Connection Manager

        private void AssignConnectionManager(ConnectionManager cmg)
        {
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(cmg);
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManagerID = cmg.ID;
            //cmg.Properties["Password"].SetValue(cmg, ConnectionPassword);
            //_connectionAssgined = true;
            //PopulateMetadata();
        }

        public void AssignConnectionManager(ISConnectionManager cm)
        {
            AssignConnectionManager(cm.ConnectionManager);
            RetrieveMetaData();
        }

        public void AssignConnectionManager(string connection)
        {
            ConnMgr = GetConnectionManager(connection);
            AssignConnectionManager(ConnMgr);
        }

        #endregion

        #region Column Mapping

        public List<ExternalColumnInputMap> ExternalColumnInputColumnMap;

        public void ManualMapToTargetColumns()
        {
            if (ExternalColumnInputColumnMap.Count > 0)
            {
                string[] viCols = new string[Input.Input.GetVirtualInput().VirtualInputColumnCollection.Count];
                for (int i = 0; i < viCols.Length; i++)
                {
                    viCols.SetValue(Input.Input.GetVirtualInput().VirtualInputColumnCollection[i].Name, i);
                }

                foreach (ExternalColumnInputMap map in ExternalColumnInputColumnMap)
                {
                    if (String.IsNullOrEmpty(map.InputColumnName))
                    {

                    }
                    else
                    {
                        ISExternalMetadataColumn extCol = new ISExternalMetadataColumn(this, DtsInput.Name, map.ExternalColumn.ExternalColumnName, true);
                        for (int vi = 0; vi < viCols.Length; vi++)
                        {
                            if (string.Equals(viCols[vi], map.InputColumnName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                ISInputColumn ic = new ISInputColumn(this, DtsInput.Name, viCols[vi], UsageType.UT_READONLY);
                                DesignTimeComponent.MapInputColumn(DtsInput.ID, ic.ID, extCol.ID);
                                extCol.DataType = ic.DataType;
                                extCol.CodePage = ic.CodePage;
                                extCol.Length = ic.Length;
                                extCol.Precision = ic.Precision;
                                extCol.Scale = ic.Scale;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #endregion

    }

}
