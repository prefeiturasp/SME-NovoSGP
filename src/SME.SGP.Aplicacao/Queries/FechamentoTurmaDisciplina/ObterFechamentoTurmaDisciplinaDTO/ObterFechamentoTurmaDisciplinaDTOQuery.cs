using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaDTOQuery : IRequest<FechamentoTurmaDisciplinaPendenciaDto>
    {
        public ObterFechamentoTurmaDisciplinaDTOQuery(string turmaCodigo, long disciplinaId, int bimestre, SituacaoFechamento[] situacoesFechamento)
        {
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
            Bimestre = bimestre;
            SituacoesFechamento = situacoesFechamento;
        }

        public string TurmaCodigo { get; set; }
        public long DisciplinaId { get; set; }
        public int Bimestre { get; set; }
        public SituacaoFechamento[] SituacoesFechamento { get; set; }

    }

    public class ObterFechamentoTurmaDisciplinaDTOQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaDTOQuery>
    {
        public ObterFechamentoTurmaDisciplinaDTOQueryValidator()
        {
            RuleFor(c => c.TurmaCodigo)
               .NotEmpty()
               .WithMessage("O id da turma deve ser informado para consulta dos fechamento.");

            RuleFor(c => c.DisciplinaId)
               .NotEmpty()
               .WithMessage("Os ids dos componentes curriculares deve ser informado para consulta do fechamento.");

            RuleFor(c => c.Bimestre)
               .NotEmpty()
               .WithMessage("O bimestre deve ser informado para consulta do fechamento.");
        }
    }
}
