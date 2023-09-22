using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresPorRfQuery : IRequest<IEnumerable<ProfessorResumoDto>>
    {
        public ObterProfessoresPorRfQuery(IEnumerable<string> codigosRF, int anoLetivo)
        {
            CodigosRF = codigosRF;
            AnoLetivo = anoLetivo;
        }

        public IEnumerable<string> CodigosRF  { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterProfessoresPorRfQueryValidator : AbstractValidator<ObterProfessoresPorRfQuery>
    {
        public ObterProfessoresPorRfQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para obter professores por Rf");
            RuleFor(x => x.CodigosRF)
                .NotNull()
                .WithMessage("A listagem de Rfs deve ser informada para obter professores por Rf");
        } 
    }
}