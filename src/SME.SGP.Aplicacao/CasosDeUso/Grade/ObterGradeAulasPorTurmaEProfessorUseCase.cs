using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGradeAulasPorTurmaEProfessorUseCase
    {
        private static IMediator _mediator;

        public static async Task<GradeComponenteTurmaAulasDto> Executar(IMediator mediator, string turmaCodigo, long disciplina, int semana, DateTime dataAula, string codigoRf = null, bool ehRegencia = false)
        {
            _mediator = mediator;

            var ue = await mediator.Send(new ObterUEPorTurmaCodigoQuery(turmaCodigo));
            if (ue == null)
                throw new NegocioException("Ue não localizada.");

            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException("Turma não localizada.");

            // Busca grade a partir dos dados da abrangencia da turma
            var grade = await mediator.Send(new ObterGradePorTipoEscolaModalidadeDuracaoQuery(ue.TipoEscola, turma.ModalidadeCodigo, turma.QuantidadeDuracaoAula));
            if (grade == null)
                return null;

            // verifica se é regencia de classe
            var horasGrade = await TratarHorasGrade(disciplina, turma, grade, ehRegencia);
            if (horasGrade == 0)
                return null;

            if (string.IsNullOrEmpty(codigoRf))
            {
                var usuario = await mediator.Send(new ObterUsuarioLogadoQuery());
                codigoRf = usuario.CodigoRf;
            }

            var horascadastradas = await ObtenhaHorasCadastradas(disciplina, semana, dataAula, codigoRf, turma, ehRegencia);
            var aulasRestantes = horasGrade - horascadastradas;

            return new GradeComponenteTurmaAulasDto
            {
                QuantidadeAulasGrade = horasGrade,
                QuantidadeAulasRestante = aulasRestantes,
                PodeEditar = aulasRestantes > 1
            };
        }

        private static async Task<int> TratarHorasGrade(long componenteCurricularId, Turma turma, GradeDto grade, bool ehRegencia)
        {
            if (ehRegencia)
                return turma.ObterHorasGradeRegencia();

            if (componenteCurricularId == 1030)
                return 4;

            return await _mediator.Send(new ObterHorasGradePorComponenteQuery(grade.Id, componenteCurricularId, int.Parse(turma.Ano)));
        }

        private static async Task<int> ObtenhaHorasCadastradas(long componenteCurricular, int semana, DateTime dataAula, string codigoRf, Turma turma, bool ehRegencia)
        {
            var experienciaPedagogica = await _mediator.Send(new AulaDeExperienciaPedagogicaQuery(componenteCurricular));

            if (ehRegencia)
                return await _mediator.Send(new ObterQuantidadeAulasNoDiaPorProfessorComponenteCurricularQuery(turma.CodigoTurma, componenteCurricular, dataAula, codigoRf, experienciaPedagogica));

            // Busca horas aula cadastradas para a disciplina na turma
            return await _mediator.Send(new ObterQuantidadeAulasNaSemanaPorProfessorComponenteCurricularQuery(turma.CodigoTurma, componenteCurricular, semana, codigoRf, experienciaPedagogica));
        }
    }
}
