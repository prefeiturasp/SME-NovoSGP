using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaAlunoAulaPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirCompensacaoAusenciaAlunoAulaPorIdCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaAlunoAulaPorIdCommand>
    {
        public ExcluirCompensacaoAusenciaAlunoAulaPorIdCommandValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da compensação de ausência aluno aula deve ser informado para exclusão lógica.");
        }
    }
}
