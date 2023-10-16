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

            var auditoria = await mediator.Send(new InserirDiarioBordoCommand(param.AulaId, param.Planejamento, param.ComponenteCurricularId));
            await mediator.Send(new ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand(param.AulaId, param.ComponenteCurricularId));
            return auditoria;
        }
        private async Task<long> RetornaComponenteCurricularIdPrincipalDoProfessor(string turmaCodigo, long componenteCurricularId)
        {
            var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(turmaCodigo, false, false, false);
            if (disciplinas.Count() > 1)
                return disciplinas.FirstOrDefault(b => b.CodigoComponenteCurricular == componenteCurricularId).CodigoComponenteCurricular;

            return disciplinas.FirstOrDefault().CodigoComponenteCurricular;
        }
    }
}
