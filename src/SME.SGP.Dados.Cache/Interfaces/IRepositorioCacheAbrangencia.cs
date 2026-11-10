namespace SME.SGP.Dominio.Interfaces
{
    /// <summary>
    /// Cache dos filtros de abrangência, em Redis próprio (Redis:Endpoint2).
    /// Sem Endpoint2 configurado, usa o Redis principal.
    /// </summary>
    public interface IRepositorioCacheAbrangencia : IRepositorioCache
    {
    }
}
