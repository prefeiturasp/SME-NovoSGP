using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoUeIgnoraGeracaoPendenciasQuery : IRequest<bool>
    {
        public ObterTipoUeIgnoraGeracaoPendenciasQuery(TipoEscola? tipoUe, string codigoUe)
        {
            TipoUe = tipoUe;
            CodigoUe = codigoUe;
        }

        public TipoEscola? TipoUe { get; }
        public string CodigoUe { get; }
    }

    public class ObterUeIgnoraGeracaoPendenciasQueryValidator : AbstractValidator<ObterTipoUeIgnoraGeracaoPendenciasQuery>
    {
        public ObterUeIgnoraGeracaoPendenciasQueryValidator()
        {
            RuleFor(a => a.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da Ue deve ser informado verificação de permissão de geração de pendência")
                .When(a => !a.TipoUe.HasValue || (int)a.TipoUe == 0);
            RuleFor(a => a.TipoUe)
                .NotEmpty()
                .WithMessage("O tipo de escola deve ser informado verificação de permissão de geração de pendência")
                .When(a => string.IsNullOrEmpty(a.CodigoUe));
        }
    }
}
