using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmasQuery : IRequest<string>
    {
        public ObterFrequenciaGeralAlunoPorTurmasQuery(string codigoAluno, string[] codigosTurmas, long tipoCalendarioId = 0, DateTime? dataMatricula = null, DateTime? dataSituacaoAluno = null)
        {
            CodigoAluno = codigoAluno;
            CodigosTurmas = codigosTurmas;
            TipoCalendarioId = tipoCalendarioId;
            DataMatriculaTurmaFiltro = dataMatricula;
            DataSituacaoAluno = dataSituacaoAluno;
        }

        public string CodigoAluno { get; }
        public string[] CodigosTurmas { get; }
        public long TipoCalendarioId { get; }
        public DateTime? DataMatriculaTurmaFiltro { get; set; }
        public DateTime? DataSituacaoAluno { get; set; }
    }

    public class ObterFrequenciaGeralAlunoPorTurmasQueryValidator : AbstractValidator<ObterFrequenciaGeralAlunoPorTurmasQuery>
    {
        public ObterFrequenciaGeralAlunoPorTurmasQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno deve ser informado para consulta de sua frequêncial anual");

            RuleFor(a => a.CodigosTurmas)
                .NotEmpty()
                .WithMessage("Os códigos de turmas devem ser informados para consulta da frequêncial anual do aluno");
        }
    }
}
