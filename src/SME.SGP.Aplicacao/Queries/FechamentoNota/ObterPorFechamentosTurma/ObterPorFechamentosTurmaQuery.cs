using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPorFechamentosTurmaQuery : IRequest<IEnumerable<FechamentoNotaAlunoAprovacaoDto>>
    {
        public long[] Ids { get; set; }
        public string CodigoTurma { get; set; }
        public string CodigoDisciplina { get; set; }
        public ObterPorFechamentosTurmaQuery(long[] ids, string codigoTurma, string codigoDisciplina)
        {
            Ids = ids;
            CodigoTurma = codigoTurma;
            CodigoDisciplina = codigoDisciplina;
        }
    }

    public class ObterPorFechamentosTurmaQueryValidator : AbstractValidator<ObterPorFechamentosTurmaQuery>
    {
        public ObterPorFechamentosTurmaQueryValidator()
        {
            RuleFor(a => a.Ids).NotEmpty().WithMessage("É necessário informar os Id(s) para obter o(s) fechamento(s) da turma");
            RuleFor(a => a.CodigoTurma).NotEmpty().WithMessage("É necessário informar o código da turma para obter o(s) fechamento(s) da turma");
            RuleFor(a => a.CodigoDisciplina).NotEmpty().WithMessage("É necessário informar o código da discplina para obter o(s) fechamento(s) da turma");
        }
    }
}
