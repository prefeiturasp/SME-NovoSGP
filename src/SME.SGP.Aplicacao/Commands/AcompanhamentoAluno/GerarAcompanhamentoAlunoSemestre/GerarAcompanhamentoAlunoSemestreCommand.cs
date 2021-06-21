using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarAcompanhamentoAlunoSemestreCommand : IRequest<AcompanhamentoAlunoSemestre>
    {
        public GerarAcompanhamentoAlunoSemestreCommand(long acompanhamentoAlunoId, int semestre, string observacoes, string percursoIndividual)
        {
            AcompanhamentoAlunoId = acompanhamentoAlunoId;
            Semestre = semestre;
            Observacoes = observacoes;
            PercursoIndividual = percursoIndividual;
        }

        public long AcompanhamentoAlunoId { get; }
        public int Semestre { get; }
        public string Observacoes { get; }
        public string PercursoIndividual { get; }
    }

    public class GerarAcompanhamentoAlunoSemestreCommandValidator : AbstractValidator<GerarAcompanhamentoAlunoSemestreCommand>
    {
        public GerarAcompanhamentoAlunoSemestreCommandValidator()
        {
            RuleFor(a => a.AcompanhamentoAlunoId)
                .NotEmpty()
                .WithMessage("O id do acompanhamento aluno deve ser informado para geração do registro do semestre");

            RuleFor(a => a.Semestre)
                .NotEmpty()
                .WithMessage("O semestre do acompanhamento aluno deve ser informado para geração do registro");
        }
    }
}
