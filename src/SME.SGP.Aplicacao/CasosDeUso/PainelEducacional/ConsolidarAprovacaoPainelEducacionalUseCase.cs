using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
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
            var listagensDres = await mediator.Send(new ObterTodasDresQuery());
            var listagemUe = (await mediator.Send(new ObterTodasUesQuery()))?.ToList();

            await mediator.Send(new LimparConsolidacaoAprovacaoCommand());
            await mediator.Send(new LimparConsolidacaoAprovacaoUeCommand());

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

                var resultadosConselho = await mediator.Send(new ObterAprovacaoParaConsolidacaoQuery([.. listagemTurmas.Select(t => t.TurmaId)]));

                var dadosParaConsolidadar = MesclarConselhosTurmas(resultadosConselho, listagemTurmas);

                var dadosAgrupadosDre = AgruparConsolicacaoDre(dadosParaConsolidadar);
                await mediator.Send(new SalvarConsolidacaoAprovacaoCommand(dadosAgrupadosDre));    
                
                var dadosAgrupadosUe = AgruparConsolicacaoUe(dadosParaConsolidadar);
                await mediator.Send(new SalvarConsolidacaoAprovacaoUeCommand(dadosAgrupadosUe));
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
                            Turma = turma.Turma,
                            SerieAno = turma.SerieAno,
                            Modalidade = turma.Modalidade.Name(),
                            AnoLetivo = turma.AnoLetivo,
                            ParecerConclusivoId = g.ParecerConclusivoId,
                            ParecerDescricao = g.ParecerDescricao
                        };
                    });

            return indicadores.ToList();
        }

        private static IEnumerable<PainelEducacionalConsolidacaoAprovacaoUe> AgruparConsolicacaoUe(IEnumerable<ConsolidacaoAprovacaoDto> dadosParaConsolidadar)
        {
            var consolidacaoAgrupada = dadosParaConsolidadar
                .GroupBy(dado => new { dado.CodigoDre, dado.Turma, dado.Modalidade, dado.AnoLetivo })
                .Select(grupo => new PainelEducacionalConsolidacaoAprovacaoUe
                {
                    CodigoDre = grupo.Key.CodigoDre,
                    Turma = grupo.Key.Turma,
                    AnoLetivo = grupo.Key.AnoLetivo,
                    Modalidade = grupo.Key.Modalidade,
                    TotalPromocoes = grupo.Count(d => PareceresPromocao.Contains(d.ParecerDescricao)),
                    TotalRetencoesNotas = grupo.Count(d => d.ParecerDescricao == ParecerRetencaoNota),
                    TotalRetencoesAusencias = grupo.Count(d => d.ParecerDescricao == ParecerRetencaoFrequencia),
                });

            return consolidacaoAgrupada;
        }

        private static IEnumerable<PainelEducacionalConsolidacaoAprovacao> AgruparConsolicacaoDre(IEnumerable<ConsolidacaoAprovacaoDto> dadosParaConsolidadar)
        {
            var consolidacaoAgrupada = dadosParaConsolidadar
                .GroupBy(dado => new { dado.CodigoDre, dado.SerieAno, dado.Modalidade, dado.AnoLetivo })
                .Select(grupo => new PainelEducacionalConsolidacaoAprovacao
                {
                    CodigoDre = grupo.Key.CodigoDre,
                    SerieAno = grupo.Key.SerieAno,
                    AnoLetivo = grupo.Key.AnoLetivo,
                    Modalidade = grupo.Key.Modalidade,
                    TotalPromocoes = grupo.Count(d => PareceresPromocao.Contains(d.ParecerDescricao)),
                    TotalRetencoesNotas = grupo.Count(d => d.ParecerDescricao == ParecerRetencaoNota),
                    TotalRetencoesAusencias = grupo.Count(d => d.ParecerDescricao == ParecerRetencaoFrequencia),
                });

            return consolidacaoAgrupada;
        }
    }
}
