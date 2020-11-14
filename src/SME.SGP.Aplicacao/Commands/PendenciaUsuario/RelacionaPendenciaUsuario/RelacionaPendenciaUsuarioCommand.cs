using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RelacionaPendenciaUsuarioCommand : IRequest<bool>
    {
        public RelacionaPendenciaUsuarioCommand(TipoParametroSistema tipoParametro, string codigoUe, long pendenciaId, long? professorId = null)
        {
            TipoParametro = tipoParametro;
            CodigoUe = codigoUe;
            PendenciaId = pendenciaId;
            ProfessorId = professorId;
        }

        public TipoParametroSistema TipoParametro { get; set; }
        public string CodigoUe { get; set; }
        public long PendenciaId { get; set; }
        public long? ProfessorId { get; set; }
    }

    public class RelacionaPendenciaUsuarioCommandValidator : AbstractValidator<RelacionaPendenciaUsuarioCommand>
    {
        public RelacionaPendenciaUsuarioCommandValidator()
        {
            RuleFor(c => c.TipoParametro)
            .NotEmpty()
            .WithMessage("O tipo do parametro sistema deve ser informado.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O codigo da Ue deve ser informado.");

            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O Id da pendencia deve ser informado.");
        }

    }
}
