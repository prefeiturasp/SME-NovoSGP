using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoParecerConclusivoCommand : IRequest
    {
        public GerarWFAprovacaoParecerConclusivoCommand(long conselhoClasseAlunoId, Turma turma, string alunoCodigo, long parecerConclusivoId, string parecerAnterior, string parecerNovo)
        {
            ConselhoClasseAlunoId = conselhoClasseAlunoId;
            Turma = turma;
            AlunoCodigo = alunoCodigo;
            ParecerConclusivoId = parecerConclusivoId;
            ParecerAnterior = parecerAnterior;
            ParecerNovo = parecerNovo;
        }

        public long ConselhoClasseAlunoId { get; }
        public Turma Turma { get; }
        public string AlunoCodigo { get; }
        public long ParecerConclusivoId { get; }
        public string ParecerAnterior { get; }
        public string ParecerNovo { get; }
    }

    public class GerarWFAprovacaoParecerConclusivoCommandValidator : AbstractValidator<GerarWFAprovacaoParecerConclusivoCommand>
    {
        public GerarWFAprovacaoParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("O identificador do conselho de classe do aluno deve ser informado para geração do workflow de aprovação do parecer conclusivo");

            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma do aluno deve ser informada para geração do workflow de aprovação do parecer conclusivo");

            RuleFor(a => a.ParecerConclusivoId)
                .NotEmpty()
                .WithMessage("O identificador do parecer conclusivo do aluno deve ser informada para geração de seu workflow de aprovação");
        }
    }
}
