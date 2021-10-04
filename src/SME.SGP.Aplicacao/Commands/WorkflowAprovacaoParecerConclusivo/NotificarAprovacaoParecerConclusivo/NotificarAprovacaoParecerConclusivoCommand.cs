using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoParecerConclusivoCommand : IRequest
    {
        public NotificarAprovacaoParecerConclusivoCommand(WFAprovacaoParecerConclusivo parecerEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome, bool aprovado, string motivo = "")
        {
            ParecerEmAprovacao = parecerEmAprovacao;
            TurmaCodigo = turmaCodigo;
            CriadorRf = criadorRf;
            CriadorNome = criadorNome;
            Aprovado = aprovado;
            Motivo = motivo;
        }

        public WFAprovacaoParecerConclusivo ParecerEmAprovacao { get; }
        public string TurmaCodigo { get; }
        public string CriadorRf { get; }
        public string CriadorNome { get; }
        public bool Aprovado { get; }
        public string Motivo { get; }
    }

    public class NotificarAprovacaoParecerConclusivoCommandValidator : AbstractValidator<NotificarAprovacaoParecerConclusivoCommand>
    {
        public NotificarAprovacaoParecerConclusivoCommandValidator()
        {
            RuleFor(a => a.ParecerEmAprovacao)
                .NotEmpty()
                .WithMessage("O parecer conclusivo em aprovação deve ser informado para notificação de sua aprovação");

            RuleFor(a => a.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma do parecer conclusivo em aprovação deve ser informado para notificação de sua aprovação");

            RuleFor(a => a.CriadorRf)
                .NotEmpty()
                .WithMessage("O RF do usuário criador do parecer conclusivo em aprovação deve ser informado para notificação de sua aprovação");

            RuleFor(a => a.CriadorNome)
                .NotEmpty()
                .WithMessage("O Nome do usuário criador do parecer conclusivo em aprovação deve ser informado para notificação de sua aprovação");
        }
    }
}
