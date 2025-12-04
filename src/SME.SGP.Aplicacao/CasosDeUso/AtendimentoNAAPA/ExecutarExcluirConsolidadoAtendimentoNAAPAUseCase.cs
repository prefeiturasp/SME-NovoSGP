using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarExcluirConsolidadoAtendimentoNAAPAUseCase : AbstractUseCase, IExecutarExcluirConsolidadoAtendimentoNAAPAUseCase

    {
        public ExecutarExcluirConsolidadoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtroExclusao = param.ObterObjetoMensagem<FiltroExcluirUesConsolidadoAtendimentoNAAPADto>();
            var idsExclusao = await mediator.Send(new ObterAtendimentosNAAPAIdConsolidadoExclusaoQuery(filtroExclusao.UeId, filtroExclusao.AnoLetivo, filtroExclusao.SituacoesIgnoradas));
            if (idsExclusao.NaoEhNulo() && idsExclusao.Any())
                foreach(var id in idsExclusao)
                    await mediator.Send(new ExcluirConsolidadoAtendimentoNAAPAPorIdCommand(id));
            return true;
        }
    }
}