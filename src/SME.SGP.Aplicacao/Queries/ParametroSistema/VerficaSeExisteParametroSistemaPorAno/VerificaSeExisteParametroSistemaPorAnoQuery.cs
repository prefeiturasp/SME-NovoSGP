using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class VerificaSeExisteParametroSistemaPorAnoQuery : IRequest<bool>
    {
        public VerificaSeExisteParametroSistemaPorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; set; }
    }

    public class VerificaSeExisteParametroSistemaPorAnoQueryValidator : AbstractValidator<VerificaSeExisteParametroSistemaPorAnoQuery>
    {
        public VerificaSeExisteParametroSistemaPorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano do parâmetro precisa ser informado");
        }
    }
}
