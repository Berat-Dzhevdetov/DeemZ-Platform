namespace DeemZ.Services
{
    using System;

    public class Guard
    {
        public bool AgainstNull(object value, string name = null)
        {
            if (value == null)
            {
                name ??= "Value";
                return false;
            }
            return true;
        }
    }
}
