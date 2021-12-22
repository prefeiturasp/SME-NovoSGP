using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNotificacoesPorAnoLetivoERfQuery : IRequest<IEnumerable<Notificacao>>
    {
        public int AnoLetivo { get; set; }
        public string UsuarioRf { get; set; }
        public int Limite { get; set; }

        public ObterNotificacoesPorAnoLetivoERfQuery(int anoLetivo, string usuarioRf, int limite)
        {
            AnoLetivo = anoLetivo;
            UsuarioRf = usuarioRf;
            Limite = limite;
        }
    }

    public class ObterNotificacoesPorAnoLetivoERfQueryValidator : AbstractValidator<ObterNotificacoesPorAnoLetivoERfQuery>
    {
        public ObterNotificacoesPorAnoLetivoERfQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.UsuarioRf)
                .NotEmpty()
                .WithMessage("O usuario deve ser informado.");

            RuleFor(x => x.Limite)
                .NotEmpty()
                .WithMessage("O limite deve ser informado.");
        }
    }
}