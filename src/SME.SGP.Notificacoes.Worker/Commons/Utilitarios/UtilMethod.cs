using System;
using System.Reflection;

namespace SME.SGP.Notificacoes.Worker
{
    public static class UtilMethod
    {
        public static MethodInfo ObterMetodo(Type objType, string method)
        {
            var methodName = objType.GetMethod(method);

            if (methodName == null)
            {
                foreach (var itf in objType.GetInterfaces())
                {
                    methodName = ObterMetodo(itf, method);

                    if (methodName != null)
                        break;
                }
            }

            return methodName;
        }
    }
}
