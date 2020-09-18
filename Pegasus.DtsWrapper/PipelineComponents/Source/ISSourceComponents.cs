using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;

namespace Pegasus.DtsWrapper.Source
{
    public class ISSourceComponent : ISPipelineComponent
    {
        #region ctor

        internal ISSourceComponent(IDTSComponentMetaData100 component) : base(component)
        {

        }

        public ISSourceComponent(ISDataFlowTask parentDataFlowTask, string componentMoniker, string componentname)
            : base(parentDataFlowTask, componentMoniker, componentname)
        {

        }

        #endregion

        #region Private properties

        internal bool _connectionAssgined = false;
        #endregion
    }
}

