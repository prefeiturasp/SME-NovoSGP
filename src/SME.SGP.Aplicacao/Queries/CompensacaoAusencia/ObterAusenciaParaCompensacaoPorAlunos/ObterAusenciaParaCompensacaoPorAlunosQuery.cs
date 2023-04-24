using System.Collections.Generic;
using System.Data;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoPorAlunosQuery :IRequest<IEnumerable<CompensacaoDataAlunoDto>>
    {
        public ObterAusenciaParaCompensacaoPorAlunosQuery(string[] codigosAlunos, string disciplinaId, int bimestre, string turmacodigo)
        {
            CodigosAlunos = codigosAlunos;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
            Turmacodigo = turmacodigo;
        }

        public string[] CodigosAlunos { get; set; }
        public string  DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public string Turmacodigo { get; set; }
    }

    public class ObterAusenciaParaCompensacaoPorAlunosQueryValidator : AbstractValidator<ObterAusenciaParaCompensacaoPorAlunosQuery>
    {
        public ObterAusenciaParaCompensacaoPorAlunosQueryValidator()
        {
            RuleFor(x => x.CodigosAlunos).NotNull().NotEmpty().WithMessage("Deve ser informado uma lista de códigos de Alunos para obter a ausência para compensação");
            RuleFor(x => x.DisciplinaId).NotNull().NotEmpty().WithMessage("Deve ser informado a disciplina para obter a ausência para compensação");
            RuleFor(x => x.Bimestre).GreaterThan(0).WithMessage("Deve ser informado o bimestre para obter a ausência para compensação");
            RuleFor(x => x.Turmacodigo).NotNull().NotEmpty().WithMessage("Deve ser informado o código da turma para obter a ausência para compensação");
        }
    }
}