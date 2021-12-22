using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralAlunoPorTurmasQuery : IRequest<FrequenciaAluno>
    {
        public ObterFrequenciaGeralAlunoPorTurmasQuery(string codigoAluno, string[] codigosTurmas, long tipoCalendarioId = 0)
        {
            CodigoAluno = codigoAluno;
            CodigosTurmas = codigosTurmas;
            TipoCalendarioId = tipoCalendarioId;
        }

        public string CodigoAluno { get; }
        public string[] CodigosTurmas { get; }
        public long TipoCalendarioId { get; }
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
