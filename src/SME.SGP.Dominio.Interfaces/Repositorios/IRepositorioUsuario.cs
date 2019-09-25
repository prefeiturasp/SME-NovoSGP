namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioUsuario : IRepositorioBase<Usuario>
    {
        Usuario ObterPorCodigoRfLogin(string codigoRf, string login);
    }
}