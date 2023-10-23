using System;
using System.Reflection;

namespace SME.SGP.Infra.Utilitarios
{
    public static class UtilMethod
    {
        public static MethodInfo ObterMetodo(Type objType, string method)
        {
            var methodName = objType.GetMethod(method);

            if (methodName is null)
            {
                foreach (var itf in objType.GetInterfaces())
                {
                    methodName = ObterMetodo(itf, method);

                    if (!(methodName is null))
                        break;
                }
            }

            return methodName;
        }
    }
}
