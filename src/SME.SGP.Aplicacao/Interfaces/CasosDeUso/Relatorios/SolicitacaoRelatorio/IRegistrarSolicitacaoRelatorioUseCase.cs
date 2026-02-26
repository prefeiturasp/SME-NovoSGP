using SME.SGP.Dominio.Dtos;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.Relatorios.SolicitacaoRelatorio
{
    public interface IRegistrarSolicitacaoRelatorioUseCase
    {
        Task Executar(SolicitacaoRelatorioDto solicitacaoRelatorio);
    }
}

