using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCurricularesPorTurmasQuery(string[] codigosTurmas)
        {
            CodigosTurmas = codigosTurmas;
        }

        public string[] CodigosTurmas { get; }
    }

    public class ObterComponentesCurricularesPorTurmasQueryValidator : AbstractValidator<ObterComponentesCurricularesPorTurmasQuery>
    {
        public ObterComponentesCurricularesPorTurmasQueryValidator()
        {
            RuleFor(a => a.CodigosTurmas)
                .NotEmpty()
                .WithMessage("Necessário informar os códigos de turmas para consulta de componentes curriculares.");
        }
    }
}