using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class RelacionaPendenciaUsuarioCommand : IRequest<bool>
    {
        public RelacionaPendenciaUsuarioCommand(string[] perfisUsuarios, string codigoUe, long pendenciaId, long? professorId = null)
        {
            PerfisUsuarios = perfisUsuarios;
            CodigoUe = codigoUe;
            PendenciaId = pendenciaId;
            ProfessorId = professorId;
        }

        public string[] PerfisUsuarios { get; set; }
        public string CodigoUe { get; set; }
        public long PendenciaId { get; set; }
        public long? ProfessorId { get; set; }
    }

    public class RelacionaPendenciaUsuarioCommandValidator : AbstractValidator<RelacionaPendenciaUsuarioCommand>
    {
        public RelacionaPendenciaUsuarioCommandValidator()
        {
            RuleFor(c => c.PerfisUsuarios)
            .NotEmpty()
            .WithMessage("Os perfis de usuário que receberão a pendência devem ser informados.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O codigo da Ue deve ser informado.");

            RuleFor(c => c.PendenciaId)
            .NotEmpty()
            .WithMessage("O Id da pendencia deve ser informado.");
        }

    }
}
