using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmaAlunoPlanoAEECommand : IRequest<bool>
    {
        public long PlanoAEEId { get; set; }
        public long TurmaId { get; set; }
    }

    public class AtualizarTurmaAlunoPlanoAEECommandValidator : AbstractValidator<AtualizarTurmaAlunoPlanoAEECommand>
    {
        public AtualizarTurmaAlunoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("É necessário informar o código do PlanoAEE para poder atualizar a turma para regular.");

            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma para poder atualizar a turma do PlanoAEE aberto");
        }
    }

}
