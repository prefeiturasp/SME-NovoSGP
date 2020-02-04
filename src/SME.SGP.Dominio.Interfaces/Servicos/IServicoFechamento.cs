using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamento
    {
        void GerarPendenciasFechamento(string disciplinaId, Turma turma, PeriodoEscolar periodoEscolar, Fechamento fechamento, Usuario usuarioLogado);

        void RealizarFechamento(string codigoTurma, string disciplinaId, long periodoEscolarId, Usuario usuarioLogado);

        Task Reprocessar(long fechamentoId);
    }
}