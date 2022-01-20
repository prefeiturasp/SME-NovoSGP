using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQuery : IRequest<bool>
    {
        public ObterIndicativoPendenciasAulasPorTipoQuery(string disciplinaId, string turmaId, TipoPendencia tipoPendenciaAula, string tabelaReferencia, long[] modalidades, int? anoLetivo = null)
        {

            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            TipoPendenciaAula = tipoPendenciaAula;
            TabelaReferencia = tabelaReferencia;
            Modalidades = modalidades;
            AnoLetivo = anoLetivo ?? DateTime.Today.Year;
        }

        public string DisciplinaId { get; set; }
        public string TurmaId { get; set; }
        public TipoPendencia TipoPendenciaAula { get; set; }
        public string TabelaReferencia { get; set; }
        public long[] Modalidades { get; set; }
        public int AnoLetivo { get; set; }
    }

    public class ObterIndicativoPendenciasAulasPorTipoQueryValidator : AbstractValidator<ObterIndicativoPendenciasAulasPorTipoQuery>
    {
        public ObterIndicativoPendenciasAulasPorTipoQueryValidator()
        {
            RuleFor(c => c.DisciplinaId)
            .NotEmpty()
            .WithMessage("O código da disciplina deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TurmaId)
            .NotEmpty()
            .WithMessage("O código da turma deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TipoPendenciaAula)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado para consulta de pendência na aula.");

            RuleFor(c => c.TabelaReferencia)
            .NotEmpty()
            .WithMessage("A tabela de referencia deve ser informada para consulta de pendência na aula.");

            RuleFor(c => c.Modalidades)
            .NotEmpty()
            .WithMessage("As modalidades deve ser informadas para consulta de pendência na aula.");

        }
    }
}
