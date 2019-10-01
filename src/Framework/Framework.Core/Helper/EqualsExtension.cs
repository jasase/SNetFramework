namespace Framework.Core.Helper
{
    public static class EqualsExtension
    {
        public static bool EqualsNullable(this object thisObj, object otherObj)
        {
            if (thisObj != null)
            {
                return thisObj.Equals(otherObj);
            }
            return otherObj == null;
        }
    }
}
