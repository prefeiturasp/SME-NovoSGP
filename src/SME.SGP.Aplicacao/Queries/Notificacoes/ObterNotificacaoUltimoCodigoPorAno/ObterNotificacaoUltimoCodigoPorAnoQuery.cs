using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{ 
    public class ObterNotificacaoUltimoCodigoPorAnoQuery : IRequest<long>
    {
        public int Ano { get; set; }

        public ObterNotificacaoUltimoCodigoPorAnoQuery(int ano)
        {
            Ano = ano;
        }
    }

    public class ObterNotificacaoUltimoCodigoPorAnoQueryValidator : AbstractValidator<ObterNotificacaoUltimoCodigoPorAnoQuery>
    {
        public ObterNotificacaoUltimoCodigoPorAnoQueryValidator()
        {
            RuleFor(x => x.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado.");
        }
    }
}