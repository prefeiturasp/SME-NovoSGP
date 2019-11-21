namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroAusenciaAluno : IRepositorioBase<RegistroAusenciaAluno>
    {
        bool MarcarRegistrosAusenciaAlunoComoExcluidoPorRegistroFrequenciaId(long registroFrequenciaId);
    }
}