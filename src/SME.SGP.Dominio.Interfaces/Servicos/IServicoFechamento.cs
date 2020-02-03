namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        void GerarPendenciasFechamento(string disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, Fechamento fechamento);

        void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId);

        void Reprocessar(long fechamentoId);
    }
}