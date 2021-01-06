using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.ObterDadosDeLeituraDeComunicadosPorModalidadeETurma;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui.DadosDeLeituraDeComunicados;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var dadosLeituraComunicadoPorModalidadeETurma = await mediator.Send(new ObterDadosDeLeituraDeComunicadosPorModalidadePorTurmaQuery(filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigoDre,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigoUe,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.NotificacaoId,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.Modalidades,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.CodigosTurmas,
                                                                                                      filtroDadosDeLeituraDeComunicadosPorModalidadeDto.ModoVisualizacao));

            
            var dadosLeituraComunicadoPorTurma =  await ObterSiglaModalidade(dadosLeituraComunicadoPorModalidadeETurma);

            return dadosLeituraComunicadoPorTurma;
        }

        private async Task<List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>> ObterSiglaModalidade(IEnumerable<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto> dadosLeituraComunicadoPorModalidadeETurma)
        {
            var dadosLeituraComunicados = new List<DadosDeLeituraDoComunicadoPorModalidadeETurmaDto>();
            foreach (var item in dadosLeituraComunicadoPorModalidadeETurma)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(item.CodigoTurma));
                if (turma == null)
                    throw new Exception("Não foi possível localizar a turma");

                item.SiglaModalidade = turma.ModalidadeCodigo.ShortName();
                dadosLeituraComunicados.Add(item);
            }
            return dadosLeituraComunicados.OrderBy(d => d.Modalidade).ThenBy(d => d.Turma).ToList();
        }
    }
}