using FluentValidation;


namespace SME.SGP.Infra.Dtos
{
    public class SalvarNotificacaoDevolutivaDto
    {
        public SalvarNotificacaoDevolutivaDto(long turmaId, string usuarioNome, string usuarioRF, long devolutivaId)
        {
            TurmaId = turmaId;
            UsuarioNome = usuarioNome;
            UsuarioRF = usuarioRF;
            DevolutivaId = devolutivaId;
        }

        public long TurmaId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRF { get; set; }
        public long DevolutivaId { get; set; }
    }

    public class SalvarNotificacaoDevolutivaDtoValidator : AbstractValidator<SalvarNotificacaoDevolutivaDto>
    {
        public SalvarNotificacaoDevolutivaDtoValidator()
        {
            RuleFor(c => c.TurmaId)
                .NotEmpty()
                .WithMessage("O id da turma deve ser informado.");

            RuleFor(c => c.UsuarioNome)
                .NotEmpty()
                .WithMessage("O nome do usuário deve ser informado.");

            RuleFor(c => c.UsuarioRF)
                .NotEmpty()
                .WithMessage("O Rf do usuario deve ser informado.");

            RuleFor(c => c.DevolutivaId)
                .NotEmpty()
                .WithMessage("O id da devolutiva deve ser informada.");
        }
    }
}
