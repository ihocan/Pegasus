using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using System;
using System.Collections.Generic;

namespace Pegasus.DtsWrapper.Destination
{
    public class ISDestinationComponent : ISPipelineComponent
    {
        #region ctor

        /// <summary>
        /// ctor that accepts a IDTSComponentMEtadata100
        /// </summary>
        /// <param name="component"></param>
        internal ISDestinationComponent(IDTSComponentMetaData100 component) : base(component)
        {

        }

        public ISDestinationComponent(ISDataFlowTask parentDataFlowTask, string componentMoniker, string componentname)
            : base(parentDataFlowTask, componentMoniker, componentname)
        {

        }

        #endregion

        #region Input

        private ISInput _input;
        public ISInput Input
        {
            get
            {
                if (_input == null)
                {
                    _input = new ISInput(this);
                }
                return _input;
            }
            set { }
        }

        internal IDTSInput100 DtsInput { get { return ComponentMetaData.InputCollection[0]; } }

        #endregion
    }



    public struct ExternalMetadataColumn
    {
        public string ExternalColumnName;
        public SSISDataType DataType;
        public int Length;
        public int Precision;
        public int Scale;
        public int CodePage;
    }

    public struct ExternalColumnInputMap
    {
        public ExternalMetadataColumn ExternalColumn { get; set; }
        public string InputColumnName { get; set; }
    }
}
