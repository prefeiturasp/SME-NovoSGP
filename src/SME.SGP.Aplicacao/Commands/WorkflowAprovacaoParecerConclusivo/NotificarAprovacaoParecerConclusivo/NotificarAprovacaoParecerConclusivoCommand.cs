using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificarAprovacaoParecerConclusivoCommand : IRequest<bool>
    {
        public NotificarAprovacaoParecerConclusivoCommand(IEnumerable<WFAprovacaoParecerConclusivoDto> pareceresEmAprovacao, string turmaCodigo, string criadorRf, string criadorNome, bool aprovado, string motivo = "")
        {
            PareceresEmAprovacao = pareceresEmAprovacao;
            TurmaCodigo = turmaCodigo;
            CriadorRf = criadorRf;
            CriadorNome = criadorNome;
            Aprovado = aprovado;
            Motivo = motivo;
        }

        public IEnumerable<WFAprovacaoParecerConclusivoDto> PareceresEmAprovacao { get; }
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
            RuleFor(a => a.PareceresEmAprovacao)
                .NotEmpty()
                .WithMessage("Os pareceres conclusivos em aprovação devem ser informados para notificação de sua aprovação");

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
