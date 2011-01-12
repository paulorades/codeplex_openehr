using System;

using OpenEhr.RM.Common.ChangeControl;
using OpenEhr.RM.Extract.Common;


namespace OpenEhr.RM.Ehr
{
    public class VersionedEhrStatus
        : VersionedObject<EhrStatus>
    {
        public VersionedEhrStatus()
            : base()
        { }

        public VersionedEhrStatus(XVersionedObject<EhrStatus> xVersionedObject)
            : base(xVersionedObject)
        { }
    }
}
