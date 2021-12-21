using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery : IRequest<FrequenciaAluno>
    {
        public string CodigoAluno { get; set; }
        public DateTime DataReferencia { get; set; }
        public TipoFrequenciaAluno Tipo { get; set; }

        public ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery(string codigoAluno, DateTime dataReferencia, TipoFrequenciaAluno tipo)
        {
            CodigoAluno = codigoAluno;
            DataReferencia = dataReferencia;
            Tipo = tipo;
        }
    }

    public class ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryValidator : AbstractValidator<ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQuery>
    {
        public ObterFrequenciaAlunosPorAlunoDataTipoFrequenciaQueryValidator()
        {
            RuleFor(x => x.CodigoAluno)
                .NotEmpty()
                .WithMessage("O codigo do aluno deve ser informado.");

            RuleFor(x => x.DataReferencia)
                .NotEmpty()
                .WithMessage("A data de referência deve ser informada.");

            RuleFor(x => x.Tipo)
                .NotEmpty()
                .WithMessage("O tipo da frequencia deve ser informado.");
        }
    }
}