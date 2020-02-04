namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamento : IRepositorioBase<Fechamento>
    {
        Fechamento ObterPorTurmaDisciplinaPeriodo(long turmaId, string disciplinaId, long periodoEscolarId);
    }
}