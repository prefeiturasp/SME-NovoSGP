namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioParametrosSistema : IRepositorioBase<ParametrosSistema>
    {
        string ObterValorPorNomeAno(string nome, int? ano);
    }
}