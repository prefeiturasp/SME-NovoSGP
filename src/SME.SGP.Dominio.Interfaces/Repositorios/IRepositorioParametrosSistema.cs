namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        string ObterValorPorTipoEAno(TipoParametroSistema tipo, int? ano = null);
    }
}