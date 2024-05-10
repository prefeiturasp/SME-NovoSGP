using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidade;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidade
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeUseCase : IObterDadosDeLeituraDeComunicadosPorModalidadeUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDeLeituraDeComunicadosPorModalidadeUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> Executar(ObterDadosDeLeituraDeComunicadosDto obterDadosDeLeituraDeComunicadosDto)
        {
            return await mediator.Send(new ObterDadosDeLeituraDeComunicadosPorModalidadeQuery(obterDadosDeLeituraDeComunicadosDto.CodigoDre, obterDadosDeLeituraDeComunicadosDto.CodigoUe, obterDadosDeLeituraDeComunicadosDto.NotificacaoId, obterDadosDeLeituraDeComunicadosDto.ModoVisualizacao));
        }
    }
}