using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentoTurmaAlunoDisciplinaQuery : IRequest<IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        public long Id { get; set; }
        public string AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }

        public ObterPorFechamentoTurmaAlunoDisciplinaQuery(long id, string alunoCodigo, long componenteCurricularId)
        {
            Id = id;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
        }
    }

    public class ObterPorFechamentoTurmaAlunoDisciplinaQueryValidator : AbstractValidator<ObterPorFechamentoTurmaAlunoDisciplinaQuery>
    {
        public ObterPorFechamentoTurmaAlunoDisciplinaQueryValidator()
        {
            RuleFor(a => a.Id).NotEmpty().WithMessage("É necessário informar o Id do Fechamento Turma para obter o(s) fechamento(s) de notas do aluno/disciplina");
            RuleFor(a => a.AlunoCodigo).NotEmpty().WithMessage("É necessário informar o código do aluno para obter o(s) fechamento(s) de notas da turma/disciplina");
            RuleFor(a => a.ComponenteCurricularId).NotEmpty().WithMessage("É necessário informar o Id do componente curricular para obter o(s) fechamento(s) de notas do aluno/turma");
        }
    }
}
