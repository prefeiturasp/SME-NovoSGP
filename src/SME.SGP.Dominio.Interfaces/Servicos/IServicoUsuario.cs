namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        void ModificarPerfil(string perfilParaModificar, string login);

        Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "");
    }
}