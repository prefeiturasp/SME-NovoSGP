namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoUsuario
    {
        Usuario ObterUsuarioPorCodigoRfLoginOuAdiciona(string codigoRf, string login = "");
    }
}