using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoUsuariosCommand : IRequest<IEnumerable<long>>
    {
        public EnviarNotificacaoUsuariosCommand(string titulo, string mensagem, NotificacaoCategoria notificacaoCategoria, NotificacaoTipo tipoNotificacao, IEnumerable<long> usuarios, string dreCodigo = "", string ueCodigo = "", string turmaCodigo = "")
        {
            Titulo = titulo;
            Mensagem = mensagem;
            CategoriaNotificacao = notificacaoCategoria;
            TipoNotificacao = tipoNotificacao;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            Usuarios = usuarios;
        }

        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public NotificacaoCategoria CategoriaNotificacao { get; set; }
        public NotificacaoTipo TipoNotificacao { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public IEnumerable<long> Usuarios { get; set; }
    }

    public class EnviarNotificacaoUsuariosCommandValidator : AbstractValidator<EnviarNotificacaoUsuariosCommand>
    {
        public EnviarNotificacaoUsuariosCommandValidator()
        {
            RuleFor(c => c.Titulo)
               .NotEmpty()
               .WithMessage("O título deve ser informado para gerar notificação.");

            RuleFor(c => c.Mensagem)
               .NotEmpty()
               .WithMessage("A mensagem deve ser informada para gerar notificação.");

            RuleFor(c => c.Usuarios)
               .NotEmpty()
               .WithMessage("Os usuarios devem ser informados para gerar notificação.");
        }
    }
}
