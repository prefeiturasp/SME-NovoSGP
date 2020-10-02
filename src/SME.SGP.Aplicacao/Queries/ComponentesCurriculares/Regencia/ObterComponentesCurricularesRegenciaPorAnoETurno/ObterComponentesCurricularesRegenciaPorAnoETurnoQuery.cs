using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorAnoETurnoQuery : IRequest<IEnumerable<ComponenteCurricularDto>>
    {
        public ObterComponentesCurricularesRegenciaPorAnoETurnoQuery(long ano, long turno)
        {
            Ano = ano;
            Turno = turno;
        }

        public long Ano { get; set; }
        public long Turno { get; set; }
    }

    public class ObterComponentesCurricularesRegenciaPorAnoETurnoQueryValidator : AbstractValidator<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>
    {
        public ObterComponentesCurricularesRegenciaPorAnoETurnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano precisa ser informado");
            RuleFor(a => a.Turno)
                .NotEmpty()
                .WithMessage("O turno precisa ser informado");
        }
    }
}
