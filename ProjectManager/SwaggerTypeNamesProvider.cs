namespace ProjectManager
{
    /// <summary>
    /// Поставщик информации об именах типов в Swagger
    /// </summary>
    public static class SwaggerTypeNamesProvider
    {
        /// <summary>
        /// Возвращает имя для типа Swagger
        /// </summary>
        /// <param name="type">Тип данных</param>
        public static string GetSwaggerDisplayedName(Type type)
        {
            return BuildDisplayedTypeName(type) ?? throw new InvalidOperationException($"Can not determine type name for type {type}");
        }

        private static string BuildDisplayedTypeName(Type type)
        {
            var prefix = string.Empty;

            if (type.DeclaringType is not null)
            {
                prefix = $"{BuildDisplayedTypeName(type.DeclaringType)}_";
            }

            if (type.IsGenericType)
            {
                var cleanTypeName = type.Name[.. ^2]; // допускаем, что больше 9 generic-параметров в типе не будет
                var genericArguments = string.Join("And", type.GenericTypeArguments.Select(BuildDisplayedTypeName));

                return $"{prefix}{cleanTypeName}Of{genericArguments}";
            }

            return $"{prefix}{type.Name}";
        }
    }
}
