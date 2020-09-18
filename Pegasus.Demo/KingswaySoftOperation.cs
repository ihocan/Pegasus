using Pegasus.DtsWrapper;
using System.IO;
using System.Collections.Generic;
using Pegasus.DtsWrapper.ConnectionManagers;
using KingswaySoft.IntegrationToolkit.DynamicsCrm;
using Pegasus.DtsWrapper.Source;
using Pegasus.DtsWrapper.Destination;

namespace Pegasus.Demo
{



    public class KingswaySoftOperation
    {
        private string _ispacFileName = "Kingsway";

        /// <summary>
        /// This example shows Kingsway Connection generate
        /// </summary>
        public void KingswayConnection()
        {
            if (File.Exists(Constants.StorageFoldePath + @"\" + _ispacFileName + ".ispac"))
                File.Delete(Constants.StorageFoldePath + @"\" + _ispacFileName + ".ispac");

            //  create a project
            ISProject mainProject = new ISProject(Constants.StorageFoldePath + @"\" + _ispacFileName + ".ispac", null);

            //  create a package
            ISPackage package = new ISPackage("ExamplePackage", mainProject);
            //  Create a Source Kinsway connection manager
            CrmSourceComponent crmSourceComponent1 = new CrmSourceComponent();

            ISKingswaySoftCRMConnectionManager crmConnection = new ISKingswaySoftCRMConnectionManager("CrmConnection",mainProject,package);
            crmConnection.ConnectionString = "";//write connection String
            crmConnection.IgnoreCertificateErrors = true;

            // Create DataFlow
            ISDataFlowTask iSDataFlowTask = new ISDataFlowTask("Data Flow", package);

            //Create Kingsway Soft Source Component
            ISCrmSourceComponent crmSourceComponent = new ISCrmSourceComponent(iSDataFlowTask, "Crm source", crmConnection);
            crmSourceComponent.SourceEntity = "account";
            crmSourceComponent.BatchSize = 1;
            crmSourceComponent.BatchSize = 1;


            //Define Output for Source Component
            ISOutput iSOutput = new ISOutput(crmSourceComponent, 0);
            //Define Entity Column for output Name doesn't need to same with crm name
            ISOutputColumn iSOutputColumn = new ISOutputColumn(crmSourceComponent, iSOutput.Name, "name");
            var datatpye = Converter.GetSSISDataTypeFromADONetDataType("string", 180);
            iSOutputColumn.SetDataTypeProperties(datatpye);

            //Configure External Metadata for Entity, columnt name must be same with crm
            ISExternalMetadataColumn iSExternalMetadataColumn = new ISExternalMetadataColumn(crmSourceComponent, iSOutput.Name, "name", false);
            iSExternalMetadataColumn.SetDataType(datatpye);
            iSExternalMetadataColumn.AssociateWithOutputColumn("name");



            //Create Destination component
            ISCrmDestination iSCrmDestination = new ISCrmDestination(iSDataFlowTask, "Crm Destination", crmSourceComponent, crmSourceComponent.GetOutputNameFromIndex(0), crmConnection);
            iSCrmDestination.DestinationEntity = "contact";
            iSCrmDestination.ActionType = (int)KingswaySoft.IntegrationToolkit.DynamicsCrm.CrmDestinationActionType.Create;



            //Create Destination Component Input
            ISInput iSInput = new ISInput(iSCrmDestination, 0);

            //Map Source component output to destination input
            var inputColumn = new ISInputColumn(iSCrmDestination, iSInput.Name, iSOutputColumn.Name, UsageType.UT_READWRITE);
            //Set input metadata
            ISExternalMetadataColumn iSExternalMetadataColumnforInput = new ISExternalMetadataColumn(iSCrmDestination, iSInput.Name, "fullname", true,"name");
            iSExternalMetadataColumnforInput.SetDataType(datatpye);
            //Set Crm type information, Type information must be defined.
            iSExternalMetadataColumnforInput.SetCustomPropertyToExternalMetadataColumn("CrmFieldType", "String");
            iSExternalMetadataColumnforInput.SetCustomPropertyToExternalMetadataColumn("LookupTypes", "");



            mainProject.SaveToDisk();


        }
    }
}
