using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarWFAprovacaoParecerConclusivoCommand : IRequest
    {
        public GerarWFAprovacaoParecerConclusivoCommand(ConselhoClasseAluno conselhoClasseAluno, Turma turma, long? parecerConclusivoId, string parecerAnterior, string parecerNovo, long usuarioSolicitanteId, bool parecerAlteradoManual = false)
        {
            ConselhoClasseAlunoId = conselhoClasseAluno.Id;
            Turma = turma;
            AlunoCodigo = conselhoClasseAluno.AlunoCodigo;
            ParecerConclusivoId = parecerConclusivoId;
            ParecerAnterior = parecerAnterior;
            ParecerNovo = parecerNovo;
            UsuarioSolicitanteId = usuarioSolicitanteId;
            ParecerConclusivoAnteriorId = conselhoClasseAluno.ConselhoClasseParecerId;
            ParecerAlteradoManual = parecerAlteradoManual;
        }

        public long ConselhoClasseAlunoId { get; }
        public Turma Turma { get; }
        public string AlunoCodigo { get; }
        public long? ParecerConclusivoId { get; }
        public long? ParecerConclusivoAnteriorId { get; }
        public string ParecerAnterior { get; }
        public string ParecerNovo { get; }
        public long UsuarioSolicitanteId { get; }
        public bool ParecerAlteradoManual { get; }
    }

    public class GerarWFAprovacaoParecerConclusivoCommandValidator : AbstractValidator<GerarWFAprovacaoParecerConclusivoCommand>
    {
        public GerarWFAprovacaoParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.ConselhoClasseAlunoId)
                .NotEmpty()
                .WithMessage("O conselho de classe do aluno deve ser informado para geração do workflow de aprovação do parecer conclusivo");

            RuleFor(a => a.Turma)
                .NotEmpty()
                .WithMessage("A turma do aluno deve ser informada para geração do workflow de aprovação do parecer conclusivo");

            RuleFor(a => a.UsuarioSolicitanteId)
                .NotEmpty()
                .WithMessage("O usuário solicitante do parecer conclusivo do aluno deve ser informada para geração de seu workflow de aprovação");
        }
    }
}
