using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalDreCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalDreCommand(long dreCodigo)
        {
            DreCodigo = dreCodigo;
        }

        public long DreCodigo { get; set; }
    }

    public class TrataSincronizacaoInstitucionalDreCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalDreCommand>
    {
        public TrataSincronizacaoInstitucionalDreCommandValidator()
        {

            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .WithMessage("O código da Dre deve ser informado.");
        }
    }
}
