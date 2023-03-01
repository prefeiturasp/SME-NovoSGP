using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciaFechamentoPorAulaQuery : IRequest<IEnumerable<PendenciaFechamento>>
    {
        public ObterPendenciaFechamentoPorAulaQuery(long idAula, TipoPendencia tipoPendencia)
        {
            IdAula = idAula;
            TipoPendencia = tipoPendencia;
        }

        public long IdAula { get; set; }
        public TipoPendencia TipoPendencia {  get; set; }
    }

    public class ObterPendenciaFechamentoPorAulaQueryValidator : AbstractValidator<ObterPendenciaFechamentoPorAulaQuery>
    {
        public ObterPendenciaFechamentoPorAulaQueryValidator()
        {
            RuleFor(c => c.IdAula)
            .NotEmpty()
            .WithMessage("O id da aula deve ser informado.");

            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado.");
        }
    }
}
