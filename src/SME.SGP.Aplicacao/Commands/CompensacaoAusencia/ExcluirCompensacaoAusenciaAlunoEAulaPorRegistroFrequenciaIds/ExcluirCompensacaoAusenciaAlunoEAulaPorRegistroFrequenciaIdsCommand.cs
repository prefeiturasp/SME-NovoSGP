using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand  : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand(IEnumerable<long> registroFrequenciaAlunoIds)
        {
            RegistroFrequenciaAlunoIds = registroFrequenciaAlunoIds;
        }

        public IEnumerable<long> RegistroFrequenciaAlunoIds { get; set; }
    }
        
    public class AlterarCompensacaoAusenciaAlunoEAulaCommandValidator: AbstractValidator<ExcluirCompensacaoAusenciaAlunoEAulaPorRegistroFrequenciaIdsCommand>
    {
        public AlterarCompensacaoAusenciaAlunoEAulaCommandValidator()
        {
            RuleFor(a => a.RegistroFrequenciaAlunoIds)
                .NotEmpty()
                .WithMessage("Os Ids de registro frequencia aluno devem ser informados para a alteração de compensação de ausência aluno e aula.");
        }
    }
}
