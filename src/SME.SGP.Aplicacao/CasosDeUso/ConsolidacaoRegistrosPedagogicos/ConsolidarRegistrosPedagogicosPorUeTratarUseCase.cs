﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosPorUeTratarUseCase : AbstractUseCase, IConsolidarRegistrosPedagogicosPorUeTratarUseCase
    {
        private readonly IRepositorioUeConsulta repositorioUe;

        public ConsolidarRegistrosPedagogicosPorUeTratarUseCase(IMediator mediator, IRepositorioUeConsulta repositorioUe) : base(mediator)
        {
            this.repositorioUe = repositorioUe;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidacaoRegistrosPedagogicosPorUeDto>();

            var dataReferencia = filtro.AnoLetivo == DateTimeExtension.HorarioBrasilia().Year
                ? DateTimeExtension.HorarioBrasilia().Date
                : new DateTime(filtro.AnoLetivo, 12, 31, 0, 0, 0, DateTimeKind.Utc);

            var consolidacoes = Enumerable.Empty<ConsolidacaoRegistrosPedagogicosDto>();

            if (await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.SepararDiarioBordoPorComponente, filtro.AnoLetivo)) != null)
            {
                var ue = await repositorioUe.ObterUePorId(filtro.UeId);
                var professoresTitulares = await mediator.Send(new ObterProfessoresTitularesPorUeQuery(ue.CodigoUe, dataReferencia));

                var professoresTitularesAgrupadoTurma = professoresTitulares.GroupBy(c => c.TurmaId);

                foreach (var agrupadoTurma in professoresTitularesAgrupadoTurma)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarRegistrosPedagogicosPorTurmaTratar, 
                        new FiltroConsolidacaoRegistrosPedagogicosPorTurmaDto(agrupadoTurma.Key.ToString(), filtro.AnoLetivo,
                        professoresTitulares), new Guid(), null));
                }
            }
            else
            {
                consolidacoes = await mediator.Send(new ObterConsolidacaoRegistrosPedagogicosQuery(filtro.UeId, filtro.AnoLetivo));

                if (consolidacoes.Any())
                {
                    var consolidacaoCompleta = await AtribuiProfessorEConsolida(consolidacoes);

                    foreach (var consolidacao in consolidacaoCompleta.Distinct())
                        await mediator.Send(new ConsolidarRegistrosPedagogicosCommand(consolidacao));
                }
            }

            return true;
        }

        public async Task<List<ConsolidacaoRegistrosPedagogicos>> AtribuiProfessorEConsolida(IEnumerable<ConsolidacaoRegistrosPedagogicosDto> consolidacoes)
        {
            string codigoTurma = string.Empty;
            var listaConsolidados = new List<ConsolidacaoRegistrosPedagogicos>();

            foreach (var consolidacaoAgrupado in consolidacoes.GroupBy(c => c.TurmaId))
            {
                var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(consolidacaoAgrupado.FirstOrDefault().TurmaCodigo));

                if (professoresDaTurma.Any())
                {
                    foreach (var bimestre in consolidacaoAgrupado.Select(c => c.PeriodoEscolarId).Distinct())
                    {
                        foreach (var professor in professoresDaTurma.GroupBy(pt => pt.DisciplinaId))
                        {
                            var dadosProfessor = professor.FirstOrDefault();
                            bool possuiConsolidacaoParaADisciplina = consolidacaoAgrupado.Any(p => p.ComponenteCurricularId == dadosProfessor.DisciplinaId && p.PeriodoEscolarId == bimestre);

                            if (possuiConsolidacaoParaADisciplina)
                            {
                                string nomeProfessor = string.Empty;
                                string rfProfessor = string.Empty;
                                bool possui2Professores = false;

                                var consolidacao = consolidacaoAgrupado.FirstOrDefault(c => c.ComponenteCurricularId == dadosProfessor.DisciplinaId && c.PeriodoEscolarId == bimestre);
                                var consolidacaoInfantil = consolidacaoAgrupado.Where(c => c.ComponenteCurricularId == dadosProfessor.DisciplinaId && c.PeriodoEscolarId == bimestre);

                                var dadosProfessorTitularDisciplina = professoresDaTurma.Where(p => p.DisciplinaId == consolidacao.ComponenteCurricularId)
                                                            .Select(pt => new ProfessorTitularDisciplinaDto()
                                                            {
                                                                RFProfessor = pt.ProfessorRf,
                                                                NomeProfessor = pt.ProfessorNome
                                                            });

                                string[] nomesProfessores = dadosProfessorTitularDisciplina.Select(dpd => dpd.NomeProfessor).ToArray();
                                string[] rfProfessores = dadosProfessorTitularDisciplina.Select(dpd => dpd.RFProfessor).ToArray();

                                if (dadosProfessorTitularDisciplina != null && consolidacao.RFProfessor != null
                                    && consolidacao.ModalidadeCodigo != (int)Modalidade.EducacaoInfantil)
                                {
                                    var dadosProfessorTitular = dadosProfessorTitularDisciplina.FirstOrDefault();

                                    await VerificaSeEhCJEAtribuiNome(dadosProfessorTitular.NomeProfessor, consolidacao.TurmaCodigo, consolidacao.RFProfessor,
                                                                      consolidacao.ComponenteCurricularId, consolidacao, dadosProfessorTitular);
                                }
                                else
                                {
                                    if (dadosProfessorTitularDisciplina != null && consolidacao.ModalidadeCodigo == (int)Modalidade.EducacaoInfantil)
                                    {
                                        if (dadosProfessorTitularDisciplina.Count() > 1)
                                        {
                                            possui2Professores = true;
                                        }
                                        else
                                        {
                                            consolidacao.NomeProfessor = dadosProfessorTitularDisciplina.FirstOrDefault().NomeProfessor;
                                            consolidacao.RFProfessor = dadosProfessorTitularDisciplina.FirstOrDefault().RFProfessor;
                                        }
                                    }
                                    else
                                    {
                                        consolidacao.NomeProfessor = "Não há professor titular";
                                        consolidacao.RFProfessor = "";
                                    }
                                }

                                if (!possui2Professores)
                                {
                                    listaConsolidados.Add(new ConsolidacaoRegistrosPedagogicos()
                                    {
                                        TurmaId = consolidacao.TurmaId,
                                        PeriodoEscolarId = consolidacao.PeriodoEscolarId,
                                        AnoLetivo = consolidacao.AnoLetivo,
                                        ComponenteCurricularId = consolidacao.ComponenteCurricularId,
                                        QuantidadeAulas = consolidacao.QuantidadeAulas,
                                        FrequenciasPendentes = consolidacao.FrequenciasPendentes,
                                        DataUltimaFrequencia = consolidacao.DataUltimaFrequencia,
                                        DataUltimoPlanoAula = consolidacao.DataUltimoPlanoAula,
                                        DataUltimoDiarioBordo = consolidacao.DataUltimoDiarioBordo,
                                        DiarioBordoPendentes = consolidacao.DiarioBordoPendentes,
                                        PlanoAulaPendentes = consolidacao.PlanoAulaPendentes,
                                        NomeProfessor = consolidacao.NomeProfessor,
                                        RFProfessor = consolidacao.RFProfessor,
                                        CJ = consolidacao.CJ
                                    });
                                }
                                else
                                {
                                    int quantidadeAulas = consolidacaoInfantil.Sum(ci => ci.QuantidadeAulas);
                                    int frequenciasPendentes = consolidacaoInfantil.Sum(ci => ci.FrequenciasPendentes);
                                    DateTime? dataUltimaFrequencia = consolidacaoInfantil.Max(ci => ci.DataUltimaFrequencia);
                                    DateTime? dataUltimoDiarioBordo = consolidacaoInfantil.Max(ci => ci.DataUltimoDiarioBordo);
                                    int diarioBordoPendentes = consolidacaoInfantil.Sum(ci => ci.DiarioBordoPendentes);

                                    for (int i = 0; i < 2; i++)
                                    {
                                        listaConsolidados.Add(new ConsolidacaoRegistrosPedagogicos()
                                        {
                                            TurmaId = consolidacao.TurmaId,
                                            PeriodoEscolarId = consolidacao.PeriodoEscolarId,
                                            AnoLetivo = consolidacao.AnoLetivo,
                                            ComponenteCurricularId = consolidacao.ComponenteCurricularId,
                                            QuantidadeAulas = quantidadeAulas,
                                            FrequenciasPendentes = frequenciasPendentes,
                                            DataUltimaFrequencia = dataUltimaFrequencia,
                                            DataUltimoPlanoAula = consolidacao.DataUltimoPlanoAula,
                                            DataUltimoDiarioBordo = dataUltimoDiarioBordo,
                                            DiarioBordoPendentes = diarioBordoPendentes,
                                            PlanoAulaPendentes = consolidacao.PlanoAulaPendentes,
                                            NomeProfessor = nomesProfessores[i],
                                            RFProfessor = rfProfessores[i],
                                            CJ = consolidacao.CJ
                                        });
                                    }
                                    possui2Professores = false;
                                }
                            }
                            else
                            {
                                var dadosConsolidadosTurma = consolidacaoAgrupado.FirstOrDefault();

                                var consolidacaoZerada = new ConsolidacaoRegistrosPedagogicos()
                                {
                                    TurmaId = dadosConsolidadosTurma.TurmaId,
                                    PeriodoEscolarId = bimestre,
                                    AnoLetivo = dadosConsolidadosTurma.AnoLetivo,
                                    ComponenteCurricularId = dadosProfessor.DisciplinaId,
                                    QuantidadeAulas = 0,
                                    FrequenciasPendentes = 0,
                                    DataUltimaFrequencia = null,
                                    DataUltimoPlanoAula = null,
                                    DataUltimoDiarioBordo = null,
                                    DiarioBordoPendentes = 0,
                                    PlanoAulaPendentes = 0,
                                    NomeProfessor = dadosProfessor.ProfessorNome,
                                    RFProfessor = dadosProfessor.ProfessorRf,
                                    CJ = false
                                };
                                listaConsolidados.Add(consolidacaoZerada);
                            }
                        }
                    }
                }
            }
            return listaConsolidados;
        }

        public async Task<string> VerificaSeEhCJEAtribuiNome(string nomeProfessor, string turmaCodigo, string rfProfessor, long disciplinaId, ConsolidacaoRegistrosPedagogicosDto consolidacao, ProfessorTitularDisciplinaDto dadosProfessorTitularDisciplina)
        {
            if (dadosProfessorTitularDisciplina.RFProfessor.Equals(consolidacao.RFProfessor))
            {
                consolidacao.NomeProfessor = dadosProfessorTitularDisciplina.NomeProfessor;
                consolidacao.RFProfessor = dadosProfessorTitularDisciplina.RFProfessor;
                consolidacao.CJ = false;
                return nomeProfessor;
            }

            bool possuiAtribuicao = await mediator.Send(new PossuiAtribuicaoCJPorTurmaRFQuery(turmaCodigo, rfProfessor, disciplinaId));
            if (possuiAtribuicao)
            {
                string nomeEol = await mediator.Send(new ObterNomeProfessorQuery(rfProfessor));
                consolidacao.NomeProfessor = nomeEol;
                consolidacao.RFProfessor = rfProfessor;
                consolidacao.CJ = true;
                return nomeEol;
            }
            else
            {
                consolidacao.NomeProfessor = dadosProfessorTitularDisciplina.NomeProfessor;
                consolidacao.RFProfessor = dadosProfessorTitularDisciplina.RFProfessor;
                consolidacao.CJ = false;
                return nomeProfessor;
            }
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ProfessoresTitularesTurma(string codigoTurma)
        {
            return await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(codigoTurma));
        }
    }
}
