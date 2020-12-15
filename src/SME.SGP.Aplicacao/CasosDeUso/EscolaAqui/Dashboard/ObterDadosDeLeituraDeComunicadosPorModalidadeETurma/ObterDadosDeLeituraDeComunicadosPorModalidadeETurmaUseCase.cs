using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.EscolaAqui.Dashboard.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma
{
    public class ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase : IObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterDadosDeLeituraDeComunicadosPorModalidadeETurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> Executar(FiltroDadosDeLeituraDeComunicadosPorModalidadeDto filtroDadosDeLeituraDeComunicadosPorModalidadeDto)
        {
            return await mediator.Send(new ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery(filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigoDre,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigoUe, 
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.NotificacaoId, 
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.Modalidades,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigoTurma,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.ModoVisualizacao));


        }
    }
}