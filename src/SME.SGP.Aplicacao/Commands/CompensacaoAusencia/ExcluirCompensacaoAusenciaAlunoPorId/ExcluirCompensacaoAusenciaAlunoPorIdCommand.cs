using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoPorIdCommand : IRequest<bool>
    {
        public ExcluirCompensacaoAusenciaAlunoPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class ExcluirCompensacaoAusenciaAlunoPorIdCommandValidator : AbstractValidator<ExcluirCompensacaoAusenciaAlunoPorIdCommand>
    {
        public ExcluirCompensacaoAusenciaAlunoPorIdCommandValidator()
        {
            RuleFor(c => c.Id)
            .NotEmpty()
            .WithMessage("O id da compensação de ausência aluno deve ser informado para exclusão lógica.");
        }
    }
}
