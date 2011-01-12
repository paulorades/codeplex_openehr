using System;
using System.Collections.Generic;
using System.Text;
using OpenEhr.AM.Archetype.ConstraintModel;
using OpenEhr.Futures.OperationalTemplate;
using OpenEhr.RM.Support.Identification;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using OpenEhr.DesignByContract;

namespace OpenEhr.Validation
{
    public delegate void AcceptValidationError(object sender, ValidationEventArgs e);
    public delegate CArchetypeRoot FetchOperationalObject(object sender, FetchOperationalObjectEventArgs e); 

    public class ValidationContext
    {
        IConfigurationSource configSource;

        internal ValidationContext() { }

        public ValidationContext(AcceptValidationError acceptErrorDelegate, FetchOperationalObject fetchObjectDelegate, IConfigurationSource configSource)
            : this(acceptErrorDelegate, fetchObjectDelegate)
        {
            Check.Require(configSource != null, "configSource must not be null.");
            this.configSource = configSource;
        }

        public ValidationContext(AcceptValidationError acceptErrorDelegate, FetchOperationalObject fetchObjectDelegate)
        {
            AcceptError = acceptErrorDelegate;
            FetchObject = fetchObjectDelegate;
        }

        public IConfigurationSource ConfigurationSource
        {
            get
            {
                return configSource != null ? configSource : ConfigurationSourceFactory.Create();
            }
        }

        public virtual FetchOperationalObject FetchObject
        {
            get; set;
        }

        public virtual AcceptValidationError AcceptError
        {
            get; set;
        }

        public virtual bool IsSuppressingAcceptErrors
        {
            get; set;
        }

        internal void AcceptValidationError(ArchetypeConstraint constraint, string message)
        {
            if (AcceptError != null && !IsSuppressingAcceptErrors)
                AcceptError(constraint, new ValidationEventArgs(constraint, message));
        }

        internal CArchetypeRoot FetchOperationalObject(ArchetypeConstraint constraint, ObjectId id)
        {
            return FetchObject != null ? FetchObject(constraint, new FetchOperationalObjectEventArgs(id)) : null;
        }
    }
}
