using System;

using OpenEhr.RM.Extract.Common;
using OpenEhr.RM.Common.ChangeControl;

namespace OpenEhr.RM.Ehr
{
    public class VersionedComposition
        : VersionedObject<Composition.Composition>
    {
        public VersionedComposition()
            : base()
        { }

        public VersionedComposition(XVersionedObject<Composition.Composition> xVersionedObject)
            : base(xVersionedObject)
        { }
    }
}
