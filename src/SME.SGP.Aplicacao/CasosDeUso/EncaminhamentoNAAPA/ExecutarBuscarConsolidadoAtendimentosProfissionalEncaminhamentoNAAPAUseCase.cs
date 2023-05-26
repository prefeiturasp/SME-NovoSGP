using System;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public class ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase : AbstractUseCase, IExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase
    {
        public ExecutarBuscarConsolidadoAtendimentosProfissionalEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPADto>();
            var atendimentosProfissional = await mediator.Send(new ObterAtendimentosProfissionalEncaminhamentosNAAPAConsolidadoCargaQuery(filtro.UeId, filtro.Mes, filtro.AnoLetivo));
            foreach (var profissional in atendimentosProfissional)
            {
                var entidade = new ConsolidadoAtendimentoNAAPA
                {
                    UeId = profissional.UeId,
                    AnoLetivo = profissional.AnoLetivo,
                    Quantidade = profissional.Quantidade,
                    Mes = profissional.Mes,
                    Profissional = profissional.Profissional
                };
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA, entidade, Guid.NewGuid()));
            }

            return true;
        }
    }
}