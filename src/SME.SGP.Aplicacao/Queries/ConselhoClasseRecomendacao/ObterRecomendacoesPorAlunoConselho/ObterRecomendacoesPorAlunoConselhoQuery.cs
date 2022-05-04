using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRecomendacoesPorAlunoConselhoQuery : IRequest<IEnumerable<RecomendacoesAlunoFamiliaDto>>
    {
        public string AlunoCodigo { get; set; }
        public int? Bimestre { get; set; }
        public long FechamentoTurmaId { get; set; }

        public ObterRecomendacoesPorAlunoConselhoQuery(string alunoCodigo, int? bimestre, long fechamentoTurmaId)
        {
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            FechamentoTurmaId = fechamentoTurmaId;
        }
    }

    public class ObterRecomendacoesPorAlunoQueryValidator : AbstractValidator<ObterRecomendacoesPorAlunoConselhoQuery>
    {
        public ObterRecomendacoesPorAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consultar as recomendações gravadas para o estudante nesse conselho de classe");

            RuleFor(a => a.FechamentoTurmaId)
               .NotEmpty()
               .WithMessage("É necessário informar a turma do conselho para consultar as recomendações gravadas para o estudante nesse conselho de classe");
        }
    }
}
