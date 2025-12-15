using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ExisteNovoEncaminhamentoNAAPAAtivoParaAluno
{
    public class ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery : IRequest<bool>
    {
        public ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; set; }
    }

    public class ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQueryValidator : AbstractValidator<ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQuery>
    {
        public ExisteNovoEncaminhamentoNAAPAAtivoParaAlunoQueryValidator()
        {
            RuleFor(c => c.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para a verificação de existência do encaminhamento naapa para o aluno.");
        }
    }
}