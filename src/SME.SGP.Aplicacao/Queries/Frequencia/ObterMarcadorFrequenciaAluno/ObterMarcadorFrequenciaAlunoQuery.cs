using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMarcadorFrequenciaAlunoQuery : IRequest<MarcadorFrequenciaDto>
    {
        public AlunoPorTurmaResposta Aluno { get; set; }
        public PeriodoEscolar PeriodoEscolar { get; set; }
        public Modalidade Modalidade { get; set; }

        public ObterMarcadorFrequenciaAlunoQuery(AlunoPorTurmaResposta aluno, PeriodoEscolar periodoEscolar, Modalidade modalidade)
        {
            Aluno = aluno;
            PeriodoEscolar = periodoEscolar;
            Modalidade = modalidade;
        }
    }

    public class ObterMarcadorFrequenciaAlunoQueryValidator : AbstractValidator<ObterMarcadorFrequenciaAlunoQuery>
    {
        public ObterMarcadorFrequenciaAlunoQueryValidator()
        {
            RuleFor(x => x.Aluno)
                .NotEmpty()
                .WithMessage("O aluno deve ser informado para consulta do marcador.");

            RuleFor(x => x.PeriodoEscolar)
                .NotEmpty()
                .WithMessage("O período escolar deve ser informado para consulta do marcador.");

            RuleFor(x => x.Modalidade)
                .NotEmpty()
                .WithMessage("A modalidade da turma deve ser informada para consulta do marcador.");
        }
    }
}