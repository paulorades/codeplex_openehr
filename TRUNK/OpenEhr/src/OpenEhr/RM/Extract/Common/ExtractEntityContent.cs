using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.RM.Common.Archetyped.Impl;

namespace OpenEhr.RM.Extract.Common
{
    /// <summary> Container of extracted and serialised content. 
    /// Intended to be subtyped into e.g. EHR_EXTRACT_CONTENT etc.
    /// </summary>
    [Serializable]
    public abstract class ExtractEntityContent : ExtractLocatable
    {
        protected ExtractEntityContent()
            : base()
        { }

        protected ExtractEntityContent(string archetypeNodeId, DataTypes.Text.DvText name)
            : base(archetypeNodeId, name)
        {
            // TODO: call SetAttributeDictionary and CheckInvariants from sub-type 
            //       after setting attribute values
        }
    }
}
