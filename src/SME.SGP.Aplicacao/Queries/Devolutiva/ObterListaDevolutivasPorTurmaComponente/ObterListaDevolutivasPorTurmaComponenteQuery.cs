using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterListaDevolutivasPorTurmaComponenteQuery : IRequest<PaginacaoResultadoDto<DevolutivaResumoDto>>
    {
        public ObterListaDevolutivasPorTurmaComponenteQuery(string turmaCodigo, long componenteCurricularCodigo, DateTime? dataReferencia)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
            DataReferencia = dataReferencia;
        }

        public string TurmaCodigo { get; set; }
        public long ComponenteCurricularCodigo { get; set; }
        public DateTime? DataReferencia { get; set; }
    }

    public class ObterListaDevolutivasPorTurmaComponenteQueryValidator : AbstractValidator<ObterListaDevolutivasPorTurmaComponenteQuery>
    {
        public ObterListaDevolutivasPorTurmaComponenteQueryValidator()
        {
            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para consulta de suas devolutivas");

            RuleFor(a => a.ComponenteCurricularCodigo)
                .NotEmpty()
                .WithMessage("O código do componente curricular deve ser informado para consulta da devolutivas da turma");
        }
    }
}
