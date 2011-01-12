using System.ComponentModel;

namespace OpenEhr.RM.Support.Identification
{
    public class TerminologyIdTypeConverter : ObjectIdTypeConverter
    {
        //public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        //{
        //    if (sourceType == typeof(string))
        //        return true;

        //    return base.CanConvertFrom(context, sourceType);
        //}

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
                                           object value)
        {
            string s = value as string;
            if (s != null)
            {
                if (TerminologyId.IsValid(s))
                    return  new TerminologyId(s);
            }
            return base.ConvertFrom(context, culture, value);
        }

        //public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
        //    object value, Type destinationType)
        //{
        //    Support.Identification.TerminologyId objectId = value as Support.Identification.TerminologyId;
        //    if (destinationType == typeof(string) && objectId != null)
        //    {
        //        if (objectId.Value == null)
        //            return "";
        //        else
        //            return objectId.Value;
        //    }

        //    return base.ConvertTo(context, culture, value, destinationType);
        //}

        //public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        //{
        //    if (destinationType == typeof(Support.Identification.TerminologyId))
        //        return true;
        //    else
        //        return base.CanConvertTo(context, destinationType);
        //}
    }
}