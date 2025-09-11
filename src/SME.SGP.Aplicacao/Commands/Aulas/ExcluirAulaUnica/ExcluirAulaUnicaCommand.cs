using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAulaUnicaCommand: IRequest<RetornoBaseDto>
    {
        public ExcluirAulaUnicaCommand(Usuario usuario, long aulaId)
        {
            Usuario = usuario;
            AulaId = aulaId;
        }

        public Usuario Usuario { get; set; }
        public long AulaId { get; set; }
    }

    public class ExcluirAulaUnicaCommandValidator: AbstractValidator<ExcluirAulaUnicaCommand>
    {
        public ExcluirAulaUnicaCommandValidator()
        {
            RuleFor(a => a.AulaId)
                .NotNull()
                .WithMessage("O Usuário deve ser informado para a exclusão de aula.");

            RuleFor(a => a.AulaId)
                .NotEmpty()
                .WithMessage("AulaId deve ser informado para a exclusão de aula.");
        }
    }
}
