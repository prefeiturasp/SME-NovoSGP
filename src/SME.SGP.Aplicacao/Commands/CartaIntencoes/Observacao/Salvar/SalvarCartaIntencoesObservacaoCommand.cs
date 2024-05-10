using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class SalvarCartaIntencoesObservacaoCommand : IRequest<AuditoriaDto>
    {
        public SalvarCartaIntencoesObservacaoCommand(long turmaId, long componenteCurricularId, long usuarioId, string observacao)
        {
            Observacao = observacao;
            ComponenteCurricularId = componenteCurricularId;
            TurmaId = turmaId;
            UsuarioId = usuarioId;
        }

        public string Observacao { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long UsuarioId { get; set; }
        public long TurmaId { get; set; }
    }

    public class SalvarCartaIntencoesObservacaoCommandValidator : AbstractValidator<SalvarCartaIntencoesObservacaoCommand>
    {
        public SalvarCartaIntencoesObservacaoCommandValidator()
        {
            RuleFor(c => c.Observacao)
               .NotEmpty()
               .WithMessage("O campo observação deve ser informado.");

            RuleFor(c => c.TurmaId)
             .NotEmpty()
             .WithMessage("O campo id da turma deve ser informado.");

            RuleFor(c => c.ComponenteCurricularId)
             .NotEmpty()
             .WithMessage("O campo id do componente curricular deve ser informado.");

            RuleFor(c => c.UsuarioId)
              .NotEmpty()
              .WithMessage("O campo id do usuário logado deve ser informado.");
        }
    }
}
