using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class NotificarObservacaoPlanoAEECommand : IRequest<bool>
    {
        public NotificarObservacaoPlanoAEECommand(long planoAEEObservacaoId, PlanoAEE planoAEE, Usuario usuarioAtual, string observacao, IEnumerable<long> usuarios)
        {
            PlanoAEEObservacaoId = planoAEEObservacaoId;
            PlanoAEEId = planoAEE.Id;
            CriadorNome = usuarioAtual.Nome;
            CriadorRF = usuarioAtual.CodigoRf;
            AlunoNome = planoAEE.AlunoNome;
            AlunoCodigo = planoAEE.AlunoCodigo;
            Observacao = observacao;
            DreAbreviacao = planoAEE.Turma.Ue.Dre.Abreviacao;
            UeNome = $"{planoAEE.Turma.Ue.TipoEscola.ShortName()} {planoAEE.Turma.Ue.Nome}";
            Usuarios = usuarios;
        }

        public long PlanoAEEObservacaoId { get; }
        public long PlanoAEEId { get; }
        public string CriadorNome { get; }
        public string CriadorRF { get; }
        public string AlunoNome { get; }
        public string AlunoCodigo { get; }
        public string Observacao { get; }
        public string DreAbreviacao { get; }
        public string UeNome { get; }
        public IEnumerable<long> Usuarios { get; }
    }

    public class NotificarObservacaoPlanoAEECommandValidator : AbstractValidator<NotificarObservacaoPlanoAEECommand>
    {
        public NotificarObservacaoPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEObservacaoId)
                .NotEmpty()
                .WithMessage("O id da observação do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.CriadorNome)
                .NotEmpty()
                .WithMessage("O nome do criador da observação do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.CriadorRF)
                .NotEmpty()
                .WithMessage("O RF do criador da observação do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.AlunoNome)
                .NotEmpty()
                .WithMessage("O nome do aluno do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O código do aluno do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.Observacao)
                .NotEmpty()
                .WithMessage("A observação do plano AEE deve ser informada para gerar a notificação ao usuário");

            RuleFor(a => a.DreAbreviacao)
                .NotEmpty()
                .WithMessage("A abreviação da DRE do plano AEE deve ser informada para gerar a notificação ao usuário");

            RuleFor(a => a.UeNome)
                .NotEmpty()
                .WithMessage("O nome da UE do plano AEE deve ser informado para gerar a notificação ao usuário");

            RuleFor(a => a.Usuarios)
                .NotEmpty()
                .WithMessage("O nome da UE do plano AEE deve ser informado para gerar a notificação ao usuário");
        }
    }
}
