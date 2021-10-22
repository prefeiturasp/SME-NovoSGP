using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosPorUeTratarUseCase : AbstractUseCase, IConsolidarRegistrosPedagogicosPorUeTratarUseCase
    {
        public ConsolidarRegistrosPedagogicosPorUeTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidacaoRegistrosPedagogicosPorUeDto>();

            var consolidacoes = await mediator.Send(new ObterConsolidacaoRegistrosPedagogicosQuery(filtro.UeId, filtro.AnoLetivo));

            if (consolidacoes.Any())
            {
                var consolidacaoCompleta = await AtribuiProfessorEConsolida(consolidacoes);

                foreach (var consolidacao in consolidacaoCompleta)
                {
                    await mediator.Send(new SalvarConsolidacaoRegistrosPedagogicosCommand(consolidacao));
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
                    foreach (var consolidacao in consolidacaoAgrupado)
                    {
                        string nomeProfessor = string.Empty;
                        string rfProfessor = string.Empty;
                        bool possui2Professores = false;

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
                            var dadosProfessor = dadosProfessorTitularDisciplina.FirstOrDefault();

                               await VerificaSeEhCJEAtribuiNome(dadosProfessor.NomeProfessor,consolidacao.TurmaCodigo, consolidacao.RFProfessor, 
                                                                 consolidacao.ComponenteCurricularId, consolidacao, dadosProfessor);
                        }
                        else
                        {
                            if (dadosProfessorTitularDisciplina != null && consolidacao.ModalidadeCodigo == (int)Modalidade.EducacaoInfantil)
                            {
                                if(dadosProfessorTitularDisciplina.Count() > 1)
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
                                ComponenteId = consolidacao.ComponenteCurricularId,
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
                            for(int i=0; i<2; i++)
                            {
                                listaConsolidados.Add(new ConsolidacaoRegistrosPedagogicos()
                                {
                                    TurmaId = consolidacao.TurmaId,
                                    PeriodoEscolarId = consolidacao.PeriodoEscolarId,
                                    AnoLetivo = consolidacao.AnoLetivo,
                                    ComponenteId = consolidacao.ComponenteCurricularId,
                                    QuantidadeAulas = consolidacao.QuantidadeAulas,
                                    FrequenciasPendentes = consolidacao.FrequenciasPendentes,
                                    DataUltimaFrequencia = consolidacao.DataUltimaFrequencia,
                                    DataUltimoPlanoAula = consolidacao.DataUltimoPlanoAula,
                                    DataUltimoDiarioBordo = consolidacao.DataUltimoDiarioBordo,
                                    DiarioBordoPendentes = consolidacao.DiarioBordoPendentes,
                                    PlanoAulaPendentes = consolidacao.PlanoAulaPendentes,
                                    NomeProfessor = nomesProfessores[i],
                                    RFProfessor = rfProfessores[i],
                                    CJ = consolidacao.CJ
                                });
                            }
                            possui2Professores = false;
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
