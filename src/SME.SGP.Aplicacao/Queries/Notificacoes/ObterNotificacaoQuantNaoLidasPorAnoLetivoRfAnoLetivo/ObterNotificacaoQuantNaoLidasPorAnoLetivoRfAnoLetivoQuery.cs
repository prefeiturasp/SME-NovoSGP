using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery : IRequest<int>
    {
        public int Ano { get; set; }
        public string UsuarioRf { get; set; }

        public ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery(int ano, string usuarioRf)
        {
            Ano = ano;
            UsuarioRf = usuarioRf;
        }
    }

    public class ObterNotificacaoQuantNaoLidasPorAnoLetivoERfanoLetivoQueryValidator : AbstractValidator<ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery>
    {
        public ObterNotificacaoQuantNaoLidasPorAnoLetivoERfanoLetivoQueryValidator()
        {
            RuleFor(x => x.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado.");

            RuleFor(x => x.UsuarioRf)
                .NotEmpty()
                .WithMessage("O usuário rf deve ser informado.");
        }
    }
}