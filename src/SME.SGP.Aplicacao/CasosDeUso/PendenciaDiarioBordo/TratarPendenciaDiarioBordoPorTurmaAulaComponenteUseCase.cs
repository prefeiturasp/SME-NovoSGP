using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase : AbstractUseCase, ITratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase
    {
        public TratarPendenciaDiarioBordoPorTurmaAulaComponenteUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroPendenciaDiarioBordoTurmaAulaDto>();

            var pendenciaProfessorDisciplinaCache = new List<PendenciaProfessorComponenteCurricularDto>();
            var turmaId = await mediator.Send(new ObterTurmaIdPorCodigoQuery(filtro.CodigoTurma));
            long pendenciaId = 0;

            foreach (var item in filtro.AulasProfessoresComponentesCurriculares)
            {
                var pendencia = pendenciaProfessorDisciplinaCache.FirstOrDefault(f => f.ComponenteCurricularId == item.ComponenteCurricularId && f.ProfessorRf.Equals(item.ProfessorRf));
                if (pendencia == null)
                {
                    pendenciaId = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(item.ComponenteCurricularId, item.ProfessorRf, item.PeriodoEscolarId, filtro.CodigoTurma));

                    if (pendenciaId == 0)
                    {
                        pendenciaId = await mediator.Send(MapearPendencia(TipoPendencia.DiarioBordo, item.DescricaoComponenteCurricular, filtro.TurmaComModalidade, filtro.NomeEscola, turmaId));
                        pendenciaProfessorDisciplinaCache.Add(new PendenciaProfessorComponenteCurricularDto()
                        {
                            ComponenteCurricularId = item.ComponenteCurricularId,
                            ProfessorRf = item.ProfessorRf,
                            PendenciaId = pendenciaId
                        });
                    }
                }
                else
                    pendenciaId = pendencia.PendenciaId;

                await mediator.Send(new SalvarPendenciaDiarioBordoCommand()
                {
                    AulaId = item.AulaId,
                    PendenciaId = pendenciaId,
                    ProfessorRf = item.ProfessorRf,
                    ComponenteCurricularId = item.ComponenteCurricularId
                });
            }
            return true;
        }

        private SalvarPendenciaCommand MapearPendencia(
                                            TipoPendencia tipoPendencia, 
                                            string descricaoComponenteCurricular, 
                                            string turmaAnoComModalidade, 
                                            string descricaoUeDre,
                                            long turmaId)
        {
            return new SalvarPendenciaCommand
            {
                TipoPendencia = tipoPendencia,
                DescricaoComponenteCurricular = descricaoComponenteCurricular,
                TurmaAnoComModalidade = turmaAnoComModalidade,
                DescricaoUeDre = descricaoUeDre,
                TurmaId = turmaId
            };
        }
    }
}
