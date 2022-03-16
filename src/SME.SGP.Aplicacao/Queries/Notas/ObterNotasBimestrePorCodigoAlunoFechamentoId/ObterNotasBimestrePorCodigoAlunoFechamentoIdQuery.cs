using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{ 
    public class ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery : IRequest<IEnumerable<FechamentoNotaDto>>
    {
        public string CodigoAluno { get; set; }
        public long FechamentoTurmaId { get; set; }

        public ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery(string codigoAluno, long fechamentoTurmaId)
        {
            CodigoAluno = codigoAluno;
            FechamentoTurmaId = fechamentoTurmaId;
        }
    }

    public class ObterNotasBimestrePorCodigoAlunoFechamentoIdQueryValidator : AbstractValidator<ObterNotasBimestrePorCodigoAlunoFechamentoIdQuery>
    {
        public ObterNotasBimestrePorCodigoAlunoFechamentoIdQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para buscar as notas do bimestre");
        }
    }
}
