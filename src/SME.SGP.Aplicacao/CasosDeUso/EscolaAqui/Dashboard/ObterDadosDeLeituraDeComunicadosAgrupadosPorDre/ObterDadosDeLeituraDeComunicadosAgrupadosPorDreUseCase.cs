using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicados;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosAgrupadosPorDre
{
    public class ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase : IObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDeLeituraDeComunicadosAgrupadosPorDreUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoDto>> Executar(ObterDadosDeLeituraDeComunicadosAgrupadosPorDreDto obterDadosDeLeituraDeComunicadosAgrupadosPorDreDto)
            => await mediator.Send(new ObterDadosDeLeituraDeComunicadosAgrupadosPorDreQuery(obterDadosDeLeituraDeComunicadosAgrupadosPorDreDto.NotificacaoId, obterDadosDeLeituraDeComunicadosAgrupadosPorDreDto.ModoVisualizacao));
    }
}