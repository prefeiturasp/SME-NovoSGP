using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaDisciplinaQuery : IRequest<FrequenciaAluno>
    {
        public string CodigoAluno { get; set; }
        public TipoFrequenciaAluno TipoFrequenciaAluno { get; set; }
        public string ComponenteCurricularId { get; set; }
        public DateTime DataReferencia { get; set; }

        public ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaDisciplinaQuery(string codigoAluno, DateTime dataReferencia, TipoFrequenciaAluno tipoFrequenciaAluno, string componenteCurricularId)
        {
            TipoFrequenciaAluno = tipoFrequenciaAluno;
            ComponenteCurricularId = componenteCurricularId;
            CodigoAluno = codigoAluno;
            DataReferencia = dataReferencia;
        }
    }

    public class ObterFrequenciaAlunosPorTurmaDisciplinaPeriodoQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaDisciplinaQuery>
    {
        public ObterFrequenciaAlunosPorTurmaDisciplinaPeriodoQueryValidator()
        {
            RuleFor(x => x.TipoFrequenciaAluno)
                .NotEmpty()
                .WithMessage("O tipo frequencia deve ser informado.");

            RuleFor(x => x.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referencia deve ser informada.");

            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O codigo do aluno deve ser informado.");

            RuleFor(x => x.ComponenteCurricularId)
                .NotEmpty()
                .WithMessage("O componente curricular deve ser informado.");
        }
    }
} 