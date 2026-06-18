using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorProfessorRfQuery : IRequest<IEnumerable<ProfessorTurmaDto>>
    {
        public ObterTurmasPorProfessorRfQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
        }

        public string CodigoRf { get; set; }
    }

    public class ObterTurmasPorProfessorRfQueryValidator : AbstractValidator<ObterTurmasPorProfessorRfQuery>
    {
        public ObterTurmasPorProfessorRfQueryValidator()
        {
            RuleFor(c => c.CodigoRf)
            .NotEmpty()
            .WithMessage("O RF do professor deve ser informado para consulta de suas turmas.");
        }
    }
}
