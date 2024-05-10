using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosSimplesDaTurmaQuery: IRequest<IEnumerable<AlunoSimplesDto>>
    {
        public ObterAlunosSimplesDaTurmaQuery(string turmaCodigo)
        {
            TurmaCodigo = turmaCodigo;
        }

        public string TurmaCodigo { get; set; }
    }

    public class ObterAlunosSimplesDaTurmaQueryValidator: AbstractValidator<ObterAlunosSimplesDaTurmaQuery>
    {
        public ObterAlunosSimplesDaTurmaQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");
        }
    }
}
