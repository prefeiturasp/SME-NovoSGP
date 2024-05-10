using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery : IRequest<IEnumerable<FechamentoTurmaDisciplina>>
    {
        public long TurmaId { get; set; }
        public long[] DisciplinasId { get; set; }
        public int Bimestre { get; set; }
        public long? TipoCalendario { get; set; }
        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery(long turmaId, long[] disciplinasId, int bimestre = 0, long? tipoCalendario = null)
        {
            TurmaId = turmaId;
            DisciplinasId = disciplinasId;
            Bimestre = bimestre;
            TipoCalendario = tipoCalendario;
        }
    }

    public class ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQuery>
    {
        public ObterFechamentoTurmaDisciplinaPorTurmaIdDisciplinasIdBimestreQueryValidator()
        {
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("É necessário informar o id da turma para obter o fechamento da turma por disciplina");

            RuleFor(a => a.DisciplinasId)
                .NotEmpty()
                .WithMessage("É necessário informar a(s) disciplina(s) para obter o fechamento da turma por disciplina");
        }
    }
}
