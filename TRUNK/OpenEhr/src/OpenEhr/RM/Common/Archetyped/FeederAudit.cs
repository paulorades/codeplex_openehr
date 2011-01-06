using System;
using OpenEhr.AssumedTypes;
using OpenEhr.RM.DataTypes.Basic;
using OpenEhr.RM.DataTypes.Encapsulated;

namespace OpenEhr.RM.Common.Archetyped
{
    [Serializable]
    public abstract class FeederAudit //: OpenEhr.RmType
    {
        //public abstract FeederAuditDetails OriginatingSystemAudit
        public abstract OpenEhr.RM.Common.Archetyped.FeederAuditDetails OriginatingSystemAudit
        {
            get;
        }

        //public abstract FeederAuditDetails FeederSystemAudit
        public abstract OpenEhr.RM.Common.Archetyped.FeederAuditDetails FeederSystemAudit
        {
            get;
        }

        public abstract List<DvIdentifier> OriginatingSystemItemIds
        {
            get;
        }

        public abstract List<DvIdentifier> FeederSystemItemIds
        {
            get;
        }

        public abstract DvEncapsulated OriginalContent
        {
            get;
            set;
        }
    }
}
