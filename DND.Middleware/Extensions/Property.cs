namespace DND.Middleware.Extensions
{
    public static class Property
    {
        public static object SetPropertyValue<TValue>(this object instance, string propertyName, TValue propertyValue)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName);
            propertyInfo?.SetValue(instance, propertyValue);
            return instance;
        }
    }
}
