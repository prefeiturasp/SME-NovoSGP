using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public int DiasAusencia { get; set; }

        public ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery(long turmaId, string turmaCodigo, int diasAusencia)
        {
            TurmaId = turmaId;
            TurmaCodigo = turmaCodigo;
            DiasAusencia = diasAusencia;
        }
    }

    public class ListarAlunosDaTurmaComAusenciaDeRegistroIndividualPorDiasQueryValidator : AbstractValidator<ListarAlunosTurmaComAusenciaRegistroIndividualPorDiasQuery>
    {
        public ListarAlunosDaTurmaComAusenciaDeRegistroIndividualPorDiasQueryValidator()
        {
            RuleFor(x => x.TurmaId)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para a busca de alunos com ausência de registro individual.");

            RuleFor(x => x.TurmaCodigo)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para a busca de alunos com ausência de registro individual.");

            RuleFor(x => x.DiasAusencia)
                .NotEmpty()
                .WithMessage("os dias de ausência devem ser informados para a busca de alunos com ausência de registro individual.");
        }
    }
}