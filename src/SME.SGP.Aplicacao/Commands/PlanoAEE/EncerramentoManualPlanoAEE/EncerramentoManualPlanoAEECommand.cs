using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class EncerramentoManualPlanoAEECommand : IRequest<RetornoEncerramentoPlanoAEEDto>
    {
        public EncerramentoManualPlanoAEECommand(long planoId)
        {
            PlanoId = planoId;
        }

        public long PlanoId { get; set; }

        public class EncerrarPlanoAeeAlunoInativoCommandValidator : AbstractValidator<EncerramentoManualPlanoAEECommand>
        {
            public EncerrarPlanoAeeAlunoInativoCommandValidator()
            {
                RuleFor(x => x.PlanoId)
                       .GreaterThan(0)
                       .WithMessage("O ID do Plano é obrigatório.");
            }
        }
    }
}
