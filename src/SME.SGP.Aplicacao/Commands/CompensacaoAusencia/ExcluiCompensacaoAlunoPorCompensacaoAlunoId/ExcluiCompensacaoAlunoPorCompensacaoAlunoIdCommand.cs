using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand : IRequest<bool>
    {
        public long CompensacaoAlunoId { get; set; }
    }

    public class ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommandValidator : AbstractValidator<ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommand>
    {
        public ExcluiCompensacaoAlunoPorCompensacaoAlunoIdCommandValidator()
        {
            RuleFor(a => a.CompensacaoAlunoId)
               .NotEmpty().WithMessage("É necessário informar o id da compensação de ausência do aluno para realizar a exclusão.");
        }
    }
}
