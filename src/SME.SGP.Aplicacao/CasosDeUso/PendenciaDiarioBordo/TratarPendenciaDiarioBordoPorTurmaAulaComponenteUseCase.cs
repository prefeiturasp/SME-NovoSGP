using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

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
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(filtro.CodigoTurma));

            if (turma.Id == 0)
                throw new NegocioException(MensagemAcompanhamentoTurma.TURMA_NAO_ENCONTRADA);

            var tipoEscola = await mediator.Send(new ObterTipoEscolaPorCodigoUEQuery(turma.Ue.CodigoUe));

            // Não gerar pendências para turmas de escolas do tipo CEI.
            var ignorarGeracaoPendencia = await mediator.Send(new ObterTipoUeIgnoraGeracaoPendenciasQuery(tipoEscola, turma.Ue.CodigoUe));
            if (ignorarGeracaoPendencia) return false;

            var pendenciaProfessorDisciplinaCache = new List<PendenciaProfessorComponenteCurricularDto>();
            long pendenciaId = 0;

            foreach (var item in filtro.AulasProfessoresComponentesCurriculares)
            {
                var pendencia = pendenciaProfessorDisciplinaCache.FirstOrDefault(f => f.ComponenteCurricularId == item.ComponenteCurricularId 
                                                                                    && f.ProfessorRf.Equals(item.ProfessorRf)
                                                                                    && f.CodigoTurma == filtro.CodigoTurma);
                if (pendencia.EhNulo())
                {
                    pendenciaId = await mediator.Send(new ObterPendenciaDiarioBordoPorComponenteProfessorPeriodoEscolarQuery(item.ComponenteCurricularId, item.ProfessorRf, item.PeriodoEscolarId, filtro.CodigoTurma));

                    if (pendenciaId == 0)
                        pendenciaId = await mediator.Send(MapearPendencia(TipoPendencia.DiarioBordo, item.DescricaoComponenteCurricular, filtro.TurmaComModalidade, filtro.NomeEscola, turma.Id));

                    pendenciaProfessorDisciplinaCache.Add(new PendenciaProfessorComponenteCurricularDto()
                    {
                        ComponenteCurricularId = item.ComponenteCurricularId,
                        ProfessorRf = item.ProfessorRf,
                        PendenciaId = pendenciaId,
                        CodigoTurma = filtro.CodigoTurma
                    });
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
