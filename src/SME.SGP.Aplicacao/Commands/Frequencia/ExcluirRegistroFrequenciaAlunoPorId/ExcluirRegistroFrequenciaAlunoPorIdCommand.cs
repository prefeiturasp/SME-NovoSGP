using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroFrequenciaAlunoPorIdCommand : IRequest<bool>
    {
        public long[] Ids { get; set; }

        public ExcluirRegistroFrequenciaAlunoPorIdCommand(long [] ids)
        {
            Ids = ids;
        }
    }

    public class ExcluirRegistroFrequenciaAlunoPorIdCommandValidator : AbstractValidator<ExcluirRegistroFrequenciaAlunoPorIdCommand>
    {
        public ExcluirRegistroFrequenciaAlunoPorIdCommandValidator()
        {
            RuleFor(a => a.Ids)
                .NotEmpty()
                .WithMessage("É necessário informar o(s) id(s) para realizar a exclusão dos registros de frequências do aluno");
        }
    }

}
