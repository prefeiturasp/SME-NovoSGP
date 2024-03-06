using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaSondagemQuery : IRequest<IEnumerable<TurmaRetornoDto>>
    {
        public ObterTurmaSondagemQuery(string codigoUe, int anoLetivo)
        {
            CodigoUe = codigoUe;
            AnoLetivo = anoLetivo;
        }

        public string CodigoUe { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterTurmaSondagemQueryValidator : AbstractValidator<ObterTurmaSondagemQuery>
    {
        public ObterTurmaSondagemQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
               .NotEmpty()
               .WithMessage("O código da ue deve ser informado para consulta das turmas de sondagem.");

            RuleFor(c => c.AnoLetivo)
               .NotEmpty()
               .WithMessage("O ano letivo deve ser informado para consulta das turmas de sondagem.");
        }
    }
}
