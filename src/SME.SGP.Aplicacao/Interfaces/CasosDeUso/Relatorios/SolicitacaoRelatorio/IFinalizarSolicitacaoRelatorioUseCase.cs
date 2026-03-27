using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public interface IFinalizarSolicitacaoRelatorioUseCase
    {
        Task Executar(long Id);
    }
}


