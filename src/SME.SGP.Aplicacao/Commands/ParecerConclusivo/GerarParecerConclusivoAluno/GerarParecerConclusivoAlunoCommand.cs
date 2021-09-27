using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class GerarParecerConclusivoAlunoCommand : IRequest<ParecerConclusivoDto>
    {
        public GerarParecerConclusivoAlunoCommand(Dominio.ConselhoClasseAluno conselhoClasseAluno)
        {
            ConselhoClasseAluno = conselhoClasseAluno;
        }

        public ConselhoClasseAluno ConselhoClasseAluno { get; }
    }

    public class GerarParecerConclusivoAlunoCommandValidator : AbstractValidator<GerarParecerConclusivoAlunoCommand>
    {
        public GerarParecerConclusivoAlunoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAluno)
                .NotEmpty()
                .WithMessage("O registro do conselho de classe do aluno deve ser informado para gerar seu parecer conclusivo");
        }
    }
}
