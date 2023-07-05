using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosQueContemPercursoIndividalPreenchidoQuery : IRequest<IEnumerable<AcompanhamentoAluno>>
    {
        public ObterAlunosQueContemPercursoIndividalPreenchidoQuery(long turmaId, int semestre)
        {
            TurmaId = turmaId;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterAlunosQueContemPercursoIndividalPreenchidoQueryValidator : AbstractValidator<ObterAlunosQueContemPercursoIndividalPreenchidoQuery>
    {
        public ObterAlunosQueContemPercursoIndividalPreenchidoQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre deve ser informado");
        }
    }
}
