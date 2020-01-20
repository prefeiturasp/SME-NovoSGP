namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamento : IRepositorioBase<Fechamento>
    {
        Fechamento ObterPorTipoCalendarioDreEUE(long tipoCalendarioId, string dreId, string ueId);
    }
}