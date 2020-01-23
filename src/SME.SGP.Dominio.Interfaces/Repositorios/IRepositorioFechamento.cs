namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamento : IRepositorioBase<Fechamento>
    {
        Fechamento ObterPorTipoCalendarioDreEUE(long tipoCalendarioId, long? dreId, long? ueId);
    }
}