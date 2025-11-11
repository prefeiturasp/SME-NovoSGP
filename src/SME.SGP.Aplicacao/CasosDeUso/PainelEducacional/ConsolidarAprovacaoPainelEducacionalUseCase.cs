using MediatR;
using SME.SGP.Aplicacao.Commands.PainelEducacional.LimparConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Commands.PainelEducacional.SalvarConsolidacaoAprovacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoParaConsolidacao;
using SME.SGP.Aplicacao.Queries.UE.ObterTodasUes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoAprovacao;
using System;
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
            try
            {
                var listagensDres = await mediator.Send(new ObterTodasDresQuery());
                var listagemUe = (await mediator.Send(new ObterTodasUesQuery()))?.ToList();

                await mediator.Send(new LimparConsolidacaoAprovacaoCommand());
                await mediator.Send(new LimparConsolidacaoAprovacaoUeCommand());

                return await ExecutarConsolidacao(listagensDres, listagemUe);
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand("Painel Educacional - Consolidação de Aprovação", LogNivel.Critico, LogContexto.WorkerPainelEducacional, ex.Message));
            }

            return false;
        }

        private async Task<bool> ExecutarConsolidacao(IEnumerable<Dre> dres, List<Ue> ues)
        {
            int anoUtilizado = DateTime.Now.Year;
            int anoMinimoConsulta = 2019;

            while (anoUtilizado >= anoMinimoConsulta)
            {
                await SalvarConsolidacaoAprovacaoPorAno(dres, ues, anoUtilizado);

                if (anoUtilizado == anoMinimoConsulta)
                    break;

                anoUtilizado--;
            }

            return true;
        }

        private async Task SalvarConsolidacaoAprovacaoPorAno(IEnumerable<Dre> dres, List<Ue> ues, int anoLetivo)
        {
            foreach (var dre in dres)
            {
                var uesDaDre = FiltrarUesValidasParaConsolidacao(dre.Id, ues);

                // obter turmas da modalidade Ensino médio, ensino fundamental e EJA
                var listagemTurmas = await mediator.Send(
                    new ObterTurmasComModalidadePorModalidadeAnoQuery(
                        anoLetivo,
                        uesDaDre.Select(u => u.Id)?.ToList(),
                        ModalidadesTurmas.Select(m => (int)m)
                    )
                );

                var resultadosConselho = await mediator.Send(new ObterAprovacaoParaConsolidacaoQuery(listagemTurmas.Select(t => t.TurmaId).ToArray()));

                var dadosParaConsolidadar = MesclarConselhosTurmas(resultadosConselho, listagemTurmas);

                var dadosAgrupadosDre = AgruparConsolicacaoDre(dadosParaConsolidadar);
                await mediator.Send(new SalvarConsolidacaoAprovacaoCommand(dadosAgrupadosDre));

                var dadosAgrupadosUe = AgruparConsolicacaoUe(dadosParaConsolidadar);
                await mediator.Send(new SalvarConsolidacaoAprovacaoUeCommand(dadosAgrupadosUe));
            }
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
                            CodigoModalidade = (int)turma.Modalidade,
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
                .GroupBy(dado => new { dado.CodigoUe, dado.Turma, dado.Modalidade, dado.AnoLetivo })
                .Select(grupo => new PainelEducacionalConsolidacaoAprovacaoUe
                {
                    CodigoDre = grupo.First().CodigoDre,
                    CodigoUe = grupo.Key.CodigoUe,
                    Turma = grupo.Key.Turma,
                    AnoLetivo = grupo.Key.AnoLetivo,
                    CodigoModalidade = grupo.First().CodigoModalidade,
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
