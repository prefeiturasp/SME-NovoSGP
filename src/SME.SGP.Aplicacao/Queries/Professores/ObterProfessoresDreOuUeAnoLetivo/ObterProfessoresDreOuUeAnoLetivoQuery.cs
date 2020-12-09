using FluentValidation;
using MediatR;
using System;
using System.Linq;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresDreOuUeAnoLetivoQuery : IRequest<IEnumerable<ProfessorEolDto>>
    {

        public ObterProfessoresDreOuUeAnoLetivoQuery(string codigoDreUe, int anoLetivo)
        {
            CodigoDreUe = codigoDreUe;
            AnoLetivo = anoLetivo;
        }

        public string CodigoDreUe { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterFuncionariosDreOuUeAnoLetivoQueryValidator : AbstractValidator<ObterProfessoresDreOuUeAnoLetivoQuery>
    {
        public ObterFuncionariosDreOuUeAnoLetivoQueryValidator()
        {
            RuleFor(c => c.CodigoDreUe)
               .NotEmpty()
               .WithMessage("O código da Dre ou Ue deve ser informado para consulta de funcionários.");

            RuleFor(c => c.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado para consulta de funcionários.");
        }
    }
}
