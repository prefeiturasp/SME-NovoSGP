using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using SME.SGP.Infra.Enumerados;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsolidarAprovacaoPainelEducacionalUseCase : ConsolidacaoBaseUseCase, IConsolidarAprovacaoPainelEducacionalUseCase
    {
        public ConsolidarAprovacaoPainelEducacionalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            // obter Dres
            var listagensDres = await mediator.Send(new ObterTodasDresQuery());
            // obter ues
            var listagemUe = (await mediator.Send(new ObterTodasUesQuery()))?.ToList();

            foreach (var dre in listagensDres)
            {
                var uesDaDre = FiltrarUesValidasParaConsolidacao(dre.Id, listagemUe);

                // obter turmas da modalidade Ensino médio e ensino fundamental
                var listagemTurmas = await mediator.Send(
                    new ObterTurmasComModalidadePorModalidadeAnoQuery(
                        2024,
                        uesDaDre.Select(u => u.Id)?.ToList(),
                        ModalidadesTurmas.Select(m => (int)m)
                    )
                );

                // obter indicadores das turmas para consolidar
                var resultadosConselho = await mediator.Send(new ObterAprovacaoParaConsolidacaoQuery([.. listagemTurmas.Select(t => t.TurmaId)]));

                var dadosParaConsolidadar = MesclarConselhosTurmas(resultadosConselho, listagemTurmas);

                var dadosAgrupadosDre = AgruparConsolicacaoDre(dadosParaConsolidadar);
                await mediator.Send(new SalvarConsolidacaoAprovacaoCommand(dadosAgrupadosDre));

                // agrupar por ano da turma

                // PainelEducacionalConsolidacaoAprovacao
                // agrupar por turma
                // salvar a consolidação por ano turma
                //var dadosAgrupados = AgruparConsolicacaoDre(dadosParaConsolidadar, listagemTurmas);
                //await mediator.Send(new SalvarConsolidacaoAprovacaoCommand(dadosAgrupados));
                // salvaar consolidação por turma
            }

            return true;
        }

        private static IEnumerable<ConsolidacaoAprovacaoDto> MesclarConselhosTurmas(IEnumerable<DadosParaConsolidarAprovacao> resultadosConselho,
                                                                                    IEnumerable<TurmaModalidadeSerieAnoDto> turmas)
        {
            var indicadores = resultadosConselho
                    .Select(g =>
                    {
                        var turma = turmas.FirstOrDefault(t => t.TurmaId == g.TurmaId);

                        return new ConsolidacaoAprovacaoDto
                        {
                            CodigoDre = turma.CodigoDre,
                            CodigoUe = turma.CodigoUe,
                            TurmaId = g.TurmaId,
                            SerieAno = turma.SerieAno,
                            Modalidade = turma.Modalidade.Name(),
                            AnoLetivo = turma.AnoLetivo,
                            ParecerConclusivoId = g.ParecerConclusivoId,
                            ParecerDescricao = g.ParecerDescricao
                        };
                    });

            return indicadores.ToList();
        }

        private static IEnumerable<PainelEducacionalConsolidacaoAprovacao> AgruparConsolicacaoDre(IEnumerable<ConsolidacaoAprovacaoDto> dadosParaConsolidadar)
        {
            var pareceresPromocao = new HashSet<string>
                {
                    "Continuidade dos estudos",
                    "Promovido",
                    "Promovido pelo conselho"
                };

            var parecerRetencaoNota = "Retido";
            var parecerRetencaoFrequencia = "Retido por frequência";

            var consolidacaoAgrupada = dadosParaConsolidadar
                .GroupBy(dado => new { dado.CodigoDre, dado.SerieAno, dado.Modalidade, dado.AnoLetivo })
                .Select(grupo => new PainelEducacionalConsolidacaoAprovacao
                {
                    CodigoDre = grupo.Key.CodigoDre,
                    SerieAno = grupo.Key.SerieAno,
                    AnoLetivo = grupo.Key.AnoLetivo,
                    Modalidade = grupo.Key.Modalidade,
                    TotalPromocoes = grupo.Count(d => pareceresPromocao.Contains(d.ParecerDescricao)),
                    TotalRetencoesNotas = grupo.Count(d => d.ParecerDescricao == parecerRetencaoNota),
                    TotalRetencoesAusencias = grupo.Count(d => d.ParecerDescricao == parecerRetencaoFrequencia),
                });

            return consolidacaoAgrupada;
        }

    }
}
