using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalTurmaCommand(TurmaParaSyncInstitucionalDto turma)
        {
            Turma = turma;
        }

        public TurmaParaSyncInstitucionalDto Turma { get; set; }
    }
    public class TrataSincronizacaoInstitucionalTurmaCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalTurmaCommand>
    {
        public TrataSincronizacaoInstitucionalTurmaCommandValidator()
        {
            RuleFor(c => c.Turma)
                .NotEmpty()
                .WithMessage("A Turma deve ser informada para sincronização.");
        }
    }
}
