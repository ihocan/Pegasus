﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;

namespace Pegasus.DtsWrapper
{
    public class ISExternalMetadataColumn
    {
        #region Properties

        #region Parent Component

        public ISPipelineComponent ParentComponent { get; set; }

        #endregion

        #region IDTSInput100 Input

        public IDTSInput100 Input
        {
            get; set;
        }
        #endregion

        #region IDTSOutput100 Output

        public IDTSOutput100 Output
        {
            get; set;
        }
        #endregion

        #region IDTSExternalMetadataColumn100 ExternalMetadataColumn

        internal IDTSExternalMetadataColumn100 ExternalMetadataColumn
        {
            get; set;
        }

        #endregion

        #region CodePage

        public int CodePage
        {
            get { return ExternalMetadataColumn.CodePage; }
            set { ExternalMetadataColumn.CodePage = value; }
        }

        #endregion

        #region DataType

        public SSISDataType DataType
        {
            get { return DtsUtility.EnumAToEnumB<Microsoft.SqlServer.Dts.Runtime.Wrapper.DataType, SSISDataType>(ExternalMetadataColumn.DataType); }
            set { ExternalMetadataColumn.DataType = DtsUtility.EnumAToEnumB<SSISDataType, DataType>(value); }
        }

        #endregion

        #region Description

        public string Description
        {
            get { return ExternalMetadataColumn.Description; }
            set { ExternalMetadataColumn.Description = value; }
        }

        #endregion

        #region ID

        public int ID
        {
            get { return ExternalMetadataColumn.ID; }
        }

        #endregion

        #region IdentificationString

        public string IdentificationString
        {
            get { return ExternalMetadataColumn.IdentificationString; }
        }

        #endregion

        #region Length

        public int Length
        {
            get { return ExternalMetadataColumn.Length; }
            set { ExternalMetadataColumn.Length = value; }
        }

        #endregion

        #region MappedColumnID

        public int MappedColumnID
        {
            get { return ExternalMetadataColumn.MappedColumnID; }
            set { ExternalMetadataColumn.MappedColumnID = value; }
        }

        #endregion

        #region Name

        public string Name
        {
            get { return ExternalMetadataColumn.Name; }
            set { ExternalMetadataColumn.Name = value; }
        }

        #endregion

        #region Precision

        public int Precision
        {
            get { return ExternalMetadataColumn.Precision; }
            set { ExternalMetadataColumn.Precision = value; }
        }

        #endregion

        #region Scale

        public int Scale
        {
            get { return ExternalMetadataColumn.Scale; }
            set { ExternalMetadataColumn.Scale = value; }
        }

        #endregion
        #endregion

        #region ctor

        public ISExternalMetadataColumn(ISPipelineComponent parentComponent, string inputOrOutputName, string externalColumnName, bool relatedToInput)
        {
            ParentComponent = parentComponent;
            if (relatedToInput)
                InitForInput(inputOrOutputName, externalColumnName);
            else
                InitForOutput(inputOrOutputName, externalColumnName);
        }

        public ISExternalMetadataColumn(ISPipelineComponent parentComponent, string inputOrOutputName, string externalColumnName, bool relatedToInput, string associateWithColumnWithName)
            : this(parentComponent, inputOrOutputName, externalColumnName, relatedToInput)
        {
            if (relatedToInput)
                AssociateWithInputColumn(associateWithColumnWithName);
            else
                AssociateWithOutputColumn(associateWithColumnWithName);
        }

        internal ISExternalMetadataColumn(IDTSExternalMetadataColumn100 externalMetadataColumn)
        {
            ExternalMetadataColumn = externalMetadataColumn;
        }

        #endregion

        #region Associate with Input

        private void InitForInput(string inputName, string externalColumnName)
        {
            Input = ParentComponent.GetInputFromName(inputName);

            bool externalColumnExists = false;
            for (int m = 0; m < Input.ExternalMetadataColumnCollection.Count; m++)
            {
                if (Input.ExternalMetadataColumnCollection[m].Name == externalColumnName)
                {
                    externalColumnExists = true;
                    ExternalMetadataColumn = Input.ExternalMetadataColumnCollection[m];
                }
            }

            if (!(externalColumnExists))
            {
                ExternalMetadataColumn = Input.ExternalMetadataColumnCollection.New();
                ExternalMetadataColumn.Name = externalColumnName;
            }
        }

        public void AssociateWithInputColumn(string inputColumnName)
        {
            IDTSInput100 input = ParentComponent.ComponentMetaData.InputCollection[0];
            IDTSVirtualInput100 vInput = input.GetVirtualInput();
            ParentComponent.DesignTimeComponent.SetUsageType(
                    input.ID,
                    vInput,
                    vInput.VirtualInputColumnCollection[inputColumnName].LineageID,
                    DTSUsageType.UT_READONLY
                    );

            IDTSInputColumn100 inputColumn = Input.InputColumnCollection[inputColumnName];
            inputColumn.ExternalMetadataColumnID = ID;
        }

        #endregion

        #region Associate with Output

        private void InitForOutput(string outputName, string externalColumnName)
        {
            Output = ParentComponent.GetOutputFromName(outputName);

            bool externalColumnExists = false;
            for (int m = 0; m < Output.ExternalMetadataColumnCollection.Count; m++)
            {
                if (Output.ExternalMetadataColumnCollection[m].Name == externalColumnName)
                {
                    externalColumnExists = true;
                    ExternalMetadataColumn = Output.ExternalMetadataColumnCollection[m];
                }
            }

            if (!(externalColumnExists))
            {
                ExternalMetadataColumn = Output.ExternalMetadataColumnCollection.New();
                ExternalMetadataColumn.Name = externalColumnName;
            }
        }

        public void AssociateWithOutputColumn(string outputColumnName)
        {
            ISOutputColumn outCol = new ISOutputColumn(ParentComponent, Output.Name, outputColumnName, RowDisposition.RD_FailComponent, RowDisposition.RD_FailComponent);
            outCol.SetDataTypeProperties(DataType, Length, Precision, Scale, CodePage);
            outCol.OutputColumn.ExternalMetadataColumnID = ID;

            IDTSOutput100 errorOutput = null;
            for (int o = 0; o < ParentComponent.ComponentMetaData.OutputCollection.Count; o++)
            {
                if (ParentComponent.ComponentMetaData.OutputCollection[o].IsErrorOut == true)
                    errorOutput = ParentComponent.ComponentMetaData.OutputCollection[o];
            }
            bool errorColExists = false;
            if (errorOutput != null)
            {
                int errorColCount = errorOutput.OutputColumnCollection.Count;
                for (int e = 0; e < errorColCount; e++)
                {
                    if (errorOutput.OutputColumnCollection[e].Name == outputColumnName)
                        errorColExists = true;
                }
                if (!(errorColExists))
                {
                    IDTSOutputColumn100 o = errorOutput.OutputColumnCollection.NewAt(errorColCount - 2);
                    o.Name = outputColumnName;
                }
            }



        }

        #endregion

        #region Set Data Type

        public void SetDataType(SSISDataType dataType, int length, int precision, int scale, int codePage)
        {
            DataType = dataType;
            Length = length;
            Precision = precision;
            Scale = scale;
            CodePage = codePage;
        }
        public void SetDataType(SSISDataTypeWithProperty sSISDataTypeWithProperty)
        {
            DataType = sSISDataTypeWithProperty.DataType;
            Length = sSISDataTypeWithProperty.Length;
            Precision = sSISDataTypeWithProperty.Precision;
            Scale = sSISDataTypeWithProperty.Scale;
            CodePage = sSISDataTypeWithProperty.CodePage;
        }

        #endregion
        #region 
        public void SetCustomPropertyToExternalMetadataColumn(string propertyName, object propertyValue, bool containsId = false)
        {
            int sourceId = 0;
            if (Input != null)
                sourceId = Input.ID;
            else
                sourceId = Output.ID;
            bool propertyExists = false;
            IDTSCustomProperty100 customProperty = null;
            for (int i = 0; i < ExternalMetadataColumn.CustomPropertyCollection.Count; i++)
            {
                if (ExternalMetadataColumn.CustomPropertyCollection[i].Name == propertyName)
                {
                    propertyExists = true;
                    break;
                }
            }
            if (propertyExists)
            {
                customProperty = ParentComponent.DesignTimeComponent.SetExternalMetadataColumnProperty(sourceId, ExternalMetadataColumn.ID, propertyName, propertyValue);
                customProperty.ContainsID = containsId;
            }
            else
            {
                customProperty = ExternalMetadataColumn.CustomPropertyCollection.New();
                customProperty.Name = propertyName;
                customProperty.Value = propertyValue;
                customProperty.ContainsID = containsId;
            }

        }
        public void SetCustomPropertyToExternalMetadataColumn(List<MetaDataColumnCustomPropety> metaDataColumnCustomPropeties)
        {
            int sourceId = 0;
            if (Input != null)
                sourceId = Input.ID;
            else
                sourceId = Output.ID;

            bool propertyExists = false;
            foreach (MetaDataColumnCustomPropety metaDataColumnCustomPropety in metaDataColumnCustomPropeties)
            {
                IDTSCustomProperty100 customProperty = null;
                for (int i = 0; i < ExternalMetadataColumn.CustomPropertyCollection.Count; i++)
                {
                    if (ExternalMetadataColumn.CustomPropertyCollection[i].Name == metaDataColumnCustomPropety.Name)
                    {
                        propertyExists = true;
                        break;
                    }
                }
                if (propertyExists)
                {
                    customProperty = ParentComponent.DesignTimeComponent.SetExternalMetadataColumnProperty(sourceId, ExternalMetadataColumn.ID, metaDataColumnCustomPropety.Name, metaDataColumnCustomPropety.Value);
                    customProperty.ContainsID = metaDataColumnCustomPropety.ContainsId;
                }
                else
                {
                    customProperty = ExternalMetadataColumn.CustomPropertyCollection.New();
                    customProperty.Name = metaDataColumnCustomPropety.Name;
                    customProperty.Value = metaDataColumnCustomPropety.Value;
                    customProperty.ContainsID = metaDataColumnCustomPropety.ContainsId;
                }
            }


        }

        public IDTSCustomProperty100 GetCustomPropertyForExternalMetadataColums(string propertyName)
        {
            for (int i = 0; i < ExternalMetadataColumn.CustomPropertyCollection.Count; i++)
            {
                if (ExternalMetadataColumn.CustomPropertyCollection[i].Name == propertyName)
                {
                    return ExternalMetadataColumn.CustomPropertyCollection[i];
                }
            }
            return null;
        }
        #endregion


    }
}
