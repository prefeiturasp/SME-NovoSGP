using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao 
{
    public class ObterNotificacaoPorCodigoQuery : IRequest<Notificacao>
    {
        public long Codigo { get; set; }

        public ObterNotificacaoPorCodigoQuery(long codigo)
        {
            Codigo = codigo;
        }
    }

    public class ObterNotificacaoPorCodigoQueryValidator : AbstractValidator<ObterNotificacaoPorCodigoQuery>
    {
        public ObterNotificacaoPorCodigoQueryValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty()
                .WithMessage("O codigo deve ser informado.");
        }
    }
}