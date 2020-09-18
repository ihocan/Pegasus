using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;

namespace Pegasus.DtsWrapper.Destination
{
    public class ISCrmDestination : ISDestinationComponent
    {
        #region ctor

        internal ISCrmDestination(IDTSComponentMetaData100 component) : base(component)
        {
            InitDefaults();
        }

        public ISCrmDestination(ISDataFlowTask parentDataFlowTask, string componentname) :
            base(parentDataFlowTask, "KingswaySoft.IntegrationToolkit.DynamicsCrm.CrmDestinationComponent", componentname)
        {
            //InitDefaults();
            //ExternalColumnInputColumnMap = new List<ExternalColumnInputMap>();

        }

        public ISCrmDestination(ISDataFlowTask parentDataFlowTask, string componentName,
            ISPipelineComponent sourceComponent,
            string sourceOutputName = "", ISConnectionManager iSConnectionManager = null, bool connectionIsOffline = false) :
            this(parentDataFlowTask, componentName)
        {
            if (String.IsNullOrEmpty(sourceOutputName))
                ConnectToAnotherPipelineComponent(sourceComponent.Name);
            else
                ConnectToAnotherPipelineComponent(sourceComponent.Name, sourceOutputName);

            if (!(connectionIsOffline))
            {
                if (iSConnectionManager != null)
                    AssignConnectionManager(iSConnectionManager);
                else
                    System.Console.WriteLine("WARN::: No Connection Manager is attached. Please use the AssignConnectionManager method to assign a Connection Manager.");
            }
            InitDefaults();

        }

        #endregion

        #region default inits

        public void InitDefaults()
        {
            _numberOfOutputsAllowed = 2;
            _numberOfInputsAllowed = 1;
            Input = new ISInput(this, 0);
            ExternalColumnInputColumnMap = new List<ExternalColumnInputMap>();
        }

        #endregion
        #region DTS Properties
        public string DestinationEntity
        {
            get { return CustomPropertyGetter<string>("DestinationEntity"); }
            set { CustomPropertySetter<string>("DestinationEntity", value); }
        }
        public int ActionType
        {
            get { return CustomPropertyGetter<int>("ActionType"); }
            set { CustomPropertySetter<int>("ActionType", value); }
        }
        public bool RemoveUnresolvableReferences
        {
            get { return CustomPropertyGetter<bool>("RemoveUnresolvableReferences"); }
            set { CustomPropertySetter<bool>("RemoveUnresolvableReferences", value); }
        }
        public bool EnableDuplicateDetection
        {
            get { return CustomPropertyGetter<bool>("EnableDuplicateDetection"); }
            set { CustomPropertySetter<bool>("EnableDuplicateDetection", value); }
        }
        public int UpsertMatchingCriteria
        {
            get { return CustomPropertyGetter<int>("UpsertMatchingCriteria"); }
            set { CustomPropertySetter<int>("UpsertMatchingCriteria", value); }
        }
        public string UpsertMatchingFields
        {
            get { return CustomPropertyGetter<string>("UpsertMatchingFields"); }
            set { CustomPropertySetter<string>("UpsertMatchingFields", value); }
        }
        public bool IgnoreNullValuedFields
        {
            get { return CustomPropertyGetter<bool>("IgnoreNullValuedFields"); }
            set { CustomPropertySetter<bool>("IgnoreNullValuedFields", value); }
        }
        public bool IgnoreUnchangedFields
        {
            get { return CustomPropertyGetter<bool>("IgnoreUnchangedFields"); }
            set { CustomPropertySetter<bool>("IgnoreUnchangedFields", value); }
        }
        public bool RemoveInvalidCharacters
        {
            get { return CustomPropertyGetter<bool>("RemoveInvalidCharacters"); }
            set { CustomPropertySetter<bool>("RemoveInvalidCharacters", value); }
        }
        public string ChangeFlagFields
        {
            get { return CustomPropertyGetter<string>("ChangeFlagFields"); }
            set { CustomPropertySetter<string>("ChangeFlagFields", value); }
        }
        public int HandlingOfMultipleMatches
        {
            get { return CustomPropertyGetter<int>("HandlingOfMultipleMatches"); }
            set { CustomPropertySetter<int>("HandlingOfMultipleMatches", value); }
        }
        public int OptimizationOnSourceDuplicates
        {
            get { return CustomPropertyGetter<int>("OptimizationOnSourceDuplicates"); }
            set { CustomPropertySetter<int>("OptimizationOnSourceDuplicates", value); }
        }
        public string Workflow
        {
            get { return CustomPropertyGetter<string>("Workflow"); }
            set { CustomPropertySetter<string>("Workflow", value); }
        }
        public int BatchSize
        {
            get { return CustomPropertyGetter<int>("BatchSize"); }
            set { CustomPropertySetter<int>("BatchSize", value); }
        }
        public int ConcurrentWritingThreadsInTotal
        {
            get { return CustomPropertyGetter<int>("ConcurrentWritingThreadsInTotal"); }
            set { CustomPropertySetter<int>("ConcurrentWritingThreadsInTotal", value); }
        }
        public bool SendDateTimeInUtcFormat
        {
            get { return CustomPropertyGetter<bool>("SendDateTimeInUtcFormat"); }
            set { CustomPropertySetter<bool>("SendDateTimeInUtcFormat", value); }
        }
        #endregion



        #region Assign Connection Manager

        private void AssignConnectionManager(ConnectionManager cmg)
        {
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManager = DtsConvert.GetExtendedInterface(cmg);
            ComponentMetaData.RuntimeConnectionCollection[0].ConnectionManagerID = cmg.ID;
        }

        public void AssignConnectionManager(ISConnectionManager cm)
        {
            AssignConnectionManager(cm.ConnectionManager);
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
                                ic.ExternalMetadataColumnID = extCol.ID;
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
        #region Mapping

        //private void PopulateMetadata()
        //{
        //    bool retrieved = RetrieveMetaData();
        //    if (retrieved)
        //    {
        //        MapInputColumnsToExternalColumns();
        //    }
        //}

        //private void MapInputColumnsToExternalColumns()
        //{
        //    if (ExternalColumnInputColumnMap.Count > 0)
        //    {
        //        string[] viCols = new string[Input.Input.GetVirtualInput().VirtualInputColumnCollection.Count];
        //        for (int i = 0; i < viCols.Length; i++)
        //        {
        //            viCols.SetValue(Input.Input.GetVirtualInput().VirtualInputColumnCollection[i].Name, i);
        //        }

        //        foreach (ExternalColumnInputMap map in ExternalColumnInputColumnMap)
        //        {
        //            for (int mc = 0; mc < DtsInput.ExternalMetadataColumnCollection.Count; mc++)
        //            {
        //                if (DtsInput.ExternalMetadataColumnCollection[mc].Name == map.ExternalColumn.ExternalColumnName)
        //                {
        //                    //IDTSInputColumn100 ic = GetInputColumn(DtsInput.Name, map.Item2);
        //                    //ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
        //                    for (int vi = 0; vi < viCols.Length; vi++)
        //                    {
        //                        if (viCols[vi] == map.InputColumnName)
        //                        {
        //                            ISInputColumn ic = new ISInputColumn(this, DtsInput.Name, viCols[vi], UsageType.UT_READONLY);
        //                            //ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
        //                            //MapInputColumn(DtsInput.Name, DtsInput.ExternalMetadataColumnCollection[mc].Name);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string[] viCols = new string[Input.Input.GetVirtualInput().VirtualInputColumnCollection.Count];
        //        for (int i = 0; i < viCols.Length; i++)
        //        {
        //            viCols.SetValue(Input.Input.GetVirtualInput().VirtualInputColumnCollection[i].Name, i);
        //        }
        //        for (int i = 0; i < viCols.Length; i++)
        //        {
        //            for (int mc = 0; mc < DtsInput.ExternalMetadataColumnCollection.Count; mc++)
        //            {
        //                if (DtsInput.ExternalMetadataColumnCollection[mc].Name == viCols[i])
        //                {
        //                    ISInputColumn ic = new ISInputColumn(this, DtsInput.Name, viCols[i], UsageType.UT_READONLY);
        //                    ic.ExternalMetadataColumnID = DtsInput.ExternalMetadataColumnCollection[mc].ID;
        //                }
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}
