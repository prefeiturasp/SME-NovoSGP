using System.Collections.Generic;
using System.Data;
using FluentValidation;
using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaParaCompensacaoPorAlunosQuery : IRequest<IEnumerable<CompensacaoDataAlunoDto>>
    {
        public ObterAusenciaParaCompensacaoPorAlunosQuery(long compensacaoAusenciaId, string[] codigosAlunos, string[] disciplinasId, int bimestre, string turmacodigo, string professor = null)
        {
            CompensacaoAusenciaId = compensacaoAusenciaId;
            CodigosAlunos = codigosAlunos;
            DisciplinasId = disciplinasId;
            Bimestre = bimestre;
            Turmacodigo = turmacodigo;
            Professor = professor;
        }
        public long CompensacaoAusenciaId { get; set; }
        public string[] CodigosAlunos { get; set; }
        public string[] DisciplinasId { get; set; }
        public int Bimestre { get; set; }
        public string Turmacodigo { get; set; }
        public string Professor { get; set; }
    }

    public class ObterAusenciaParaCompensacaoPorAlunosQueryValidator : AbstractValidator<ObterAusenciaParaCompensacaoPorAlunosQuery>
    {
        public ObterAusenciaParaCompensacaoPorAlunosQueryValidator()
        {
            RuleFor(x => x.CompensacaoAusenciaId).GreaterThan(0).WithMessage("Deve ser informado uma lista de códigos de Alunos para obter a ausência para compensação");
            RuleFor(x => x.CodigosAlunos).NotNull().NotEmpty().WithMessage("Deve ser informado uma lista de códigos de Alunos para obter a ausência para compensação");
            RuleFor(x => x.DisciplinasId).NotNull().NotEmpty().WithMessage("Deve ser informado a disciplina para obter a ausência para compensação");
            RuleFor(x => x.Bimestre).GreaterThan(0).WithMessage("Deve ser informado o bimestre para obter a ausência para compensação");
            RuleFor(x => x.Turmacodigo).NotNull().NotEmpty().WithMessage("Deve ser informado o código da turma para obter a ausência para compensação");
        }
    }
}