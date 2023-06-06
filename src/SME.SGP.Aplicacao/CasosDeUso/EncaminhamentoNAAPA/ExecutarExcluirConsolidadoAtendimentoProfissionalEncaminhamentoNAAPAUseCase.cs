using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase

    {
        public ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtroExclusao = param.ObterObjetoMensagem<FiltroExcluirAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto>();
            var idsExclusao = await mediator.Send(new ObterProfissionaisAtendimentoEncaminhamentosNAAPAIdConsolidadoExclusaoQuery(filtroExclusao.UeId, filtroExclusao.Mes, filtroExclusao.AnoLetivo, filtroExclusao.RfsProfissionaisIgnorados));
            if (idsExclusao != null && idsExclusao.Any())
                foreach (var id in idsExclusao)
                    await mediator.Send(new ExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPAPorIdCommand(id));
            return true;
        }
    }
}