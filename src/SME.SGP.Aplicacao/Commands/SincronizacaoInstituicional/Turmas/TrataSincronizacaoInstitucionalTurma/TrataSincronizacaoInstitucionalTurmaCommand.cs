using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaCommand : IRequest<bool>
    {
        public TrataSincronizacaoInstitucionalTurmaCommand(TurmaParaSyncInstitucionalDto turmaEol, Turma turmaSGP)
        {
            TurmaEOL = turmaEol;
            TurmaSGP = turmaSGP;
        }

        public TurmaParaSyncInstitucionalDto TurmaEOL { get; set; }
        public Turma TurmaSGP { get; set; }
    }
    public class TrataSincronizacaoInstitucionalTurmaCommandValidator : AbstractValidator<TrataSincronizacaoInstitucionalTurmaCommand>
    {
        public TrataSincronizacaoInstitucionalTurmaCommandValidator()
        {
            RuleFor(c => c.TurmaEOL)
                .NotEmpty()
                .WithMessage("A Turma deve ser informada para sincronização.");
        }
    }
}
