using System;
using System.Collections.Generic;
using System.Text;

namespace OpenEhr.RM.Common.ChangeControl
{
    public interface IVersion
    {
        Support.Identification.ObjectVersionId Uid
        {
            get;
            //set;
        }

        Support.Identification.ObjectVersionId PrecedingVersionUid
        {
            get;
            //set;
        }

        DataTypes.Text.DvCodedText LifecycleState
        {
            get;
            //set;
        }

        object Data
        {
            get;
        }

        string CanonicalForm { get; }
        string Signature { get; }

        Support.Identification.HierObjectId OwnerId { get; }

       // EhrTypes.FEEDER_AUDIT CreateAudit();
        bool IsBranch { get; }

        Support.Identification.ObjectRef Contribution { get; }

        Generic.AuditDetails CommitAudit { get; }

       // List<EhrTypes.ATTESTATION> Attestations
       // {
       //     get;
       //     set;
       // }
    }

    public interface IVersion<T>
    {
        Support.Identification.ObjectVersionId Uid
        {
            get;
            //set;
        }

        Support.Identification.ObjectVersionId PrecedingVersionUid
        {
            get;
            //set;
        }

        DataTypes.Text.DvCodedText LifecycleState
        {
            get;
            //set;
        }

        T Data
        {
            get;
        }

        string CanonicalForm { get; }
        string Signature { get; }

        Support.Identification.HierObjectId OwnerId { get; }

        // EhrTypes.FEEDER_AUDIT CreateAudit();
        bool IsBranch { get; }

        Support.Identification.ObjectRef Contribution { get; }

        Generic.AuditDetails CommitAudit { get; }

        // List<EhrTypes.ATTESTATION> Attestations
        // {
        //     get;
        //     set;
        // }
    }
}
