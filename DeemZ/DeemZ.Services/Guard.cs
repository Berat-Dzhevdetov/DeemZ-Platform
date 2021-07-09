namespace DeemZ.Services
{
    public class Guard
    {
        public bool AgainstNull(object value, string name = null)
        {
            if (value == null)
            {
                name ??= "Value";
                return true;
            }
            return false;
        }
    }
}
