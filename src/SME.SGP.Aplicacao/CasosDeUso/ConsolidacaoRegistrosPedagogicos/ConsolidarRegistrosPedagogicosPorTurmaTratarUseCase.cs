using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase : AbstractUseCase, IConsolidarRegistrosPedagogicosPorTurmaTratarUseCase
    {
        public ConsolidarRegistrosPedagogicosPorTurmaTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidacaoRegistrosPedagogicosPorTurmaDto>();
            var componentesCurriculares = (from ptd in filtro.ProfessorTitularDisciplinaEols
                                           from d in ptd.DisciplinasId()
                                           select d).ToArray();

            var consolidacoes = await mediator.Send(new ObterConsolidacaoRegistrosComSeparacaoDiarioBordoQuery(filtro.TurmaCodigo, 
                filtro.AnoLetivo, componentesCurriculares));

            foreach(var consolidacao in consolidacoes.Distinct())
            {
                if (consolidacao.ModalidadeCodigo == (int)Modalidade.EducacaoInfantil)
                {
                    var professor = filtro.ProfessorTitularDisciplinaEols.FirstOrDefault(c => c.DisciplinasId().Contains(consolidacao.ComponenteCurricularId));

                    if (!string.IsNullOrEmpty(professor.ProfessorRf))
                    {
                        consolidacao.RFProfessor = professor.ProfessorRf;
                        consolidacao.NomeProfessor = professor.ProfessorNome;
                    }
                }

                await mediator.Send(new ConsolidarRegistrosPedagogicosCommand(new ConsolidacaoRegistrosPedagogicos()
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
                }));
            }

            return true;
        }
    }
}
