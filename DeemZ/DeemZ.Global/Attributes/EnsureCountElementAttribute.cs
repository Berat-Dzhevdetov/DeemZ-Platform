namespace DeemZ.Global.Attributes
{
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;

    public class EnsureCountElementAttribute : ValidationAttribute
    {
        readonly int count;

        public EnsureCountElementAttribute(int count) => this.count = count;

        public override bool IsValid(object value)
        {
            var list = value as IList;

            if (list != null)
            {
                return list.Count >= count;
            }

            return false;
        }

        bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}