using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery : IRequest<IEnumerable<FechamentoTurmaDisciplinaPendenciaDto>>
    {
        public ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery(long idUe, SituacaoFechamento[] situacoesFechamento, long[] idsFechamentoTurmaDisciplinaIgnorados = null)
        {
            IdUe = idUe;
            SituacoesFechamento = situacoesFechamento;
            IdsFechamentoTurmaDisciplinaIgnorados = idsFechamentoTurmaDisciplinaIgnorados;
        }

        public long[] IdsFechamentoTurmaDisciplinaIgnorados { get; set; }
        public SituacaoFechamento[] SituacoesFechamento { get; set; }
        
        public long IdUe { get; set; }

    }

    public class ObterFechamentoTurmaDisciplinaDTOPorSituacaoQueryValidator : AbstractValidator<ObterFechamentoTurmaDisciplinaAnoAtualDTOPorUeSituacaoQuery>
    {
        public ObterFechamentoTurmaDisciplinaDTOPorSituacaoQueryValidator()
        {
            RuleFor(c => c.SituacoesFechamento)
               .NotEmpty()
               .WithMessage("Situações de fechamento da turma/disciplina devem ser informados para consulta dos fechamentos.");
            RuleFor(c => c.IdUe)
               .NotEmpty()
               .WithMessage("O Id da Ue deve ser informado para consulta dos fechamentos.");

        }
    }
}
