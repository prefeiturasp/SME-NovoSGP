using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAlunosPorTurmaNaUEQuery : IRequest<IEnumerable<AlunosPorTurmaDto>>
    {
        public ObterQuantidadeAlunosPorTurmaNaUEQuery(string ueCodigo)
        {
            UeCodigo = ueCodigo;
        }

        public string UeCodigo { get; }
    }

    public class ObterQuantidadeAlunosPorTurmaNaUEQueryValidator : AbstractValidator<ObterQuantidadeAlunosPorTurmaNaUEQuery>
    {
        public ObterQuantidadeAlunosPorTurmaNaUEQueryValidator()
        {
            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta da quantidade de alunos nas turmas");
        }
    }
}
