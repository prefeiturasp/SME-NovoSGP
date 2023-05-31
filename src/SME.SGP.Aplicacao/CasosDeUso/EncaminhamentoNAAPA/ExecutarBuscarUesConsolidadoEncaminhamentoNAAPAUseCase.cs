using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase: AbstractUseCase,IExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase
    {
        public ExecutarBuscarUesConsolidadoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroBuscarUesConsolidadoEncaminhamentoNAAPADto>();
            var encaminhamentos = await mediator.Send(new ObterEncaminhamentosNAAPAConsolidadoCargaQuery(filtro.UeId,filtro.AnoLetivo));
            foreach (var encaminhamento in encaminhamentos)
            {
                var entidade = new ConsolidadoEncaminhamentoNAAPA
                {
                    UeId = encaminhamento.UeId,
                    AnoLetivo = encaminhamento.AnoLetivo,
                    Quantidade = encaminhamento.Quantidade,
                    Situacao = encaminhamento.Situacao,
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoEncaminhamentoNAAPA, entidade, Guid.NewGuid()));
            }

            return true;
        }
    }
}