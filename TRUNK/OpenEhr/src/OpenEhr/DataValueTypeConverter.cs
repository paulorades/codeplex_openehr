using System;
using System.ComponentModel;
using System.Globalization;

namespace OpenEhr.RM.DataTypes.Text.Impl
{
    public class CodePhraseTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string)value;

                    string[] parts = s.Split(':');

                    DataTypes.Text.CodePhrase codePhrase = new DataTypes.Text.CodePhrase(parts[2], parts[0]);

                    return codePhrase;
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type DV_CODE_PHRASE");
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
            object value, Type destinationType)
        {

            DataTypes.Text.CodePhrase codePhrase = value as DataTypes.Text.CodePhrase;
            if (destinationType == typeof(string) && codePhrase != null)
            {

                string s = codePhrase.TerminologyId.Value + "::" + codePhrase.CodeString;
                if (s == "::")
                    return "";

                return s;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(DataTypes.Text.CodePhrase))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }
    }

    public class TextValueTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            //throw new NotImplementedException("CanConvertFrom not implemented");

            if (sourceType == typeof(string))
                return true;

            if (sourceType.IsAssignableFrom(typeof(DataTypes.Text.DvText)))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
            object value)
        {
            if (value != null)
            {
                try
                {
                    if (value is string)
                    {
                        string s = (string)value;
                        string[] parts = s.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length == 3)
                        {
                            DataTypes.Text.DvCodedText codedValue
                                = new DataTypes.Text.DvCodedText(parts[2], parts[1], parts[0]);

                            return codedValue;
                        }

                        if (parts.Length == 2 && parts[0] == "local")
                        {
                            DataTypes.Text.DvCodedText codedValue
                                = new DataTypes.Text.DvCodedText("", parts[1], parts[0]);

                            return codedValue;
                        }

                        DesignByContract.Check.Assert(parts.Length <= 1, "Invalid text string: " + s);

                        DataTypes.Text.DvText textValue = new DataTypes.Text.DvText(s);

                        return textValue;
                    }

                    if (value.GetType().IsAssignableFrom(typeof(DataTypes.Text.DvText)))
                    {
                        return value;
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException("Can not convert '" + (string)value + "' to type DvText", ex);
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
            object value, Type destinationType)
        {
            DataTypes.Text.DvText textValue = value as DataTypes.Text.DvText;
            if (textValue != null)
            {
                //if (destinationType == typeof(string) && textValue != null)
                if (destinationType == typeof(string))
                {
                    DataTypes.Text.DvCodedText codedValue = textValue as DataTypes.Text.DvCodedText;

                    if (codedValue == null)
                        return textValue.Value;

                    else
                    {
                        string s = codedValue.DefiningCode.TerminologyId.Value + "::" + codedValue.DefiningCode.CodeString
                            + "::" + codedValue.Value;

                        //if (s == "::::")
                        //    return "";
                        s = s.TrimEnd(':');

                        return s;
                    }
                }

                if (destinationType == typeof(System.Object))
                {
                    return textValue as object;
                }

                if (destinationType == textValue.GetType())
                //else if (textValue.GetType().IsAssignableFrom(destinationType))
                {
                    return textValue;
                    //DataTypes.Text.DvCodedText codedValue = textValue as DataTypes.Text.DvCodedText;
                    //if (codedValue == null)
                    //    return new DataTypes.Text.DvText(textValue.Value);

                    //else
                    //{
                    //    return new DataTypes.Text.DvCodedText(codedValue.Value, 
                    //        codedValue.DefiningCode.CodeString, codedValue.DefiningCode.TerminologyId.Value);
                    //}
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType.IsAssignableFrom(typeof(DataTypes.Text.DvText)))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }
    }
}

namespace OpenEhr.RM.Support.Identification
{
    public class ObjectIdTypeConverter : ExpandableObjectConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string valueString = value as string;
            if (valueString != null)
            {
                if (HierObjectId.IsValid(valueString))
                    return new HierObjectId(valueString);                    
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture,
            object value, Type destinationType)
        {
            Support.Identification.ObjectId objectId = value as Support.Identification.ObjectId;
            if (destinationType == typeof(string) && objectId != null)
            {
                if (objectId.Value == null)
                    return "";
                else
                    return objectId.Value;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            //if (destinationType == typeof(Support.Identification.ObjectId))
            if (destinationType.IsAssignableFrom(typeof(Support.Identification.ObjectId)))
                return true;
            else
                return base.CanConvertTo(context, destinationType);
        }
    }

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
