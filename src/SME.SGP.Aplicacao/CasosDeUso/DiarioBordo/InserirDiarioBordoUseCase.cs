using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirDiarioBordoUseCase : AbstractUseCase, IInserirDiarioBordoUseCase
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        public InserirDiarioBordoUseCase(IMediator mediator, IConsultasDisciplina consultasDisciplina) : base(mediator)
        {
            this.consultasDisciplina = consultasDisciplina ??
                throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AuditoriaDto> Executar(InserirDiarioBordoDto param)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(param.AulaId));
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));

            if (turma == null)
                throw new NegocioException("Turma informada não encontrada");

            param.ComponenteCurricularId = await RetornaComponenteCurricularIdPrincipalDoProfessor(turma.CodigoTurma, param.ComponenteCurricularId);

            if (param.ComponenteCurricularId == 0)
                throw new NegocioException($"Componente Curricular não encontrado");

            var auditoria = await mediator.Send(new InserirDiarioBordoCommand(param.AulaId, param.Planejamento, param.ComponenteCurricularId));
            await mediator.Send(new ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand(param.AulaId, param.ComponenteCurricularId));
            return auditoria;
        }
        private async Task<long> RetornaComponenteCurricularIdPrincipalDoProfessor(string turmaCodigo, long componenteCurricularId)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false, false, false);
            if (disciplinas != null && disciplinas.Any())
            {
                if (disciplinas.Count() > 1)
                {
                    var disciplina = disciplinas.Where(b => b.CodigoComponenteCurricular == componenteCurricularId);

                    if (disciplina != null)
                        return 0;

                    if (disciplina.Any())
                        return disciplina.FirstOrDefault().CodigoComponenteCurricular;
                    else
                        return (long)disciplinas.FirstOrDefault().CdComponenteCurricularPai;
                }

                return disciplinas.FirstOrDefault().CodigoComponenteCurricular;
            }
            return 0;
        }
    }
}
