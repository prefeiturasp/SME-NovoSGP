using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    public static class ObjectExtension
    {
        public static bool EhNulo(this object objeto) /* assertNull */
        {
            return objeto is null;
        }

        public static bool NaoEhNulo(this object objeto) /* assertNotNull */
        {
            return !(objeto is null);
        }

        public static void LancarExcecaoNegocioSeEhNulo(this object objeto, string msgErro)
        {
            if (objeto.EhNulo())
                throw new NegocioException(msgErro);
        }
    }
}
