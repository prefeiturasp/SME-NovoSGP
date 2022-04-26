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
        public int Bimestre { get; set; }
        public long TurmaId { get; set; }

        public ObterRecomendacoesPorAlunoConselhoQuery(string alunoCodigo, int bimestre, long turmaId)
        {
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            TurmaId = turmaId;
        }
    }

    public class ObterRecomendacoesPorAlunoQueryValidator : AbstractValidator<ObterRecomendacoesPorAlunoConselhoQuery>
    {
        public ObterRecomendacoesPorAlunoQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("É necessário informar o código do aluno para consultar as recomendações gravadas para ele nesse conselho de classe");
        }
    }
}
