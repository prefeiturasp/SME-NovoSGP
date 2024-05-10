using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados
{
    public class ObterDadosDeLeituraDeComunicadosUseCase : IObterDadosDeLeituraDeComunicadosUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDeLeituraDeComunicadosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoDto>> Executar(ObterDadosDeLeituraDeComunicadosDto obterDadosDeLeituraDeComunicadosDto) 
            => await mediator.Send(new ObterDadosDeLeituraDeComunicadosQuery(obterDadosDeLeituraDeComunicadosDto.CodigoDre, obterDadosDeLeituraDeComunicadosDto.CodigoUe, obterDadosDeLeituraDeComunicadosDto.NotificacaoId, obterDadosDeLeituraDeComunicadosDto.ModoVisualizacao, obterDadosDeLeituraDeComunicadosDto.AgruparModalidade));
    }
}