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

            var consolidacaoCompleta = await AtribuiProfessorEConsolida(consolidacoes);

            foreach(var consolidacao in consolidacaoCompleta)
            {
                await mediator.Send(new SalvarConsolidacaoRegistrosPedagogicosCommand(consolidacao));
            }

            return true;
        }

        public async Task<List<ConsolidacaoRegistrosPedagogicos>> AtribuiProfessorEConsolida(IEnumerable<ConsolidacaoRegistrosPedagogicosDto> consolidacoes)
        {
            string codigoTurma = string.Empty;
            var listaConsolidados = new List<ConsolidacaoRegistrosPedagogicos>();
            var professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(consolidacoes.FirstOrDefault().TurmaCodigo));

            foreach (var consolidacao in consolidacoes)
            {
                if (!codigoTurma.Equals(consolidacao.TurmaCodigo))
                {
                    codigoTurma = consolidacao.TurmaCodigo;
                    professoresDaTurma = await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(codigoTurma));
                }
                else
                {
                    var dadosProfessorDisciplina = professoresDaTurma.Where(p => p.DisciplinaId == consolidacao.ComponenteCurricularId)
                                                .Select(pt => new ProfessorTitularDisciplinaDto()
                                                {
                                                    RFProfessor = pt.ProfessorRf,
                                                    NomeProfessor = pt.ProfessorNome
                                                }).FirstOrDefault();

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
                        NomeProfessor = dadosProfessorDisciplina.NomeProfessor,
                        RFProfessor = dadosProfessorDisciplina.RFProfessor
                    });
                }
            }

            return listaConsolidados;
        }

        public async Task<IEnumerable<ProfessorTitularDisciplinaEol>> ProfessoresTitularesTurma(string codigoTurma)
        {
            return await mediator.Send(new ObterProfessoresTitularesDaTurmaCompletosQuery(codigoTurma));
        }
    }
}
