using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class GerarNotificacaoCommand : IRequest<bool>
    {
        public GerarNotificacaoCommand(int ano, NotificacaoCategoria categoria, string dreId, string mensagem, long? usuarioId, NotificacaoTipo tipo, string titulo, string turmaId, string ueId)
        {
            Titulo = titulo;
            Mensagem = mensagem;
            Ano = ano;
            Categoria = categoria;
            DreId = dreId;
            UsuarioId = usuarioId;
            Tipo = tipo;
            TurmaId = turmaId;
            UeId = ueId;
        }

        public int Ano { get; set; }
        public NotificacaoCategoria Categoria { get; set; }
        public string DreId { get; set; }
        public string Mensagem { get; set; }
        public long? UsuarioId { get; set; }
        public NotificacaoTipo Tipo { get; set; }
        public string Titulo { get; set; }
        public string TurmaId { get; set; }
        public string UeId { get; set; }
    }

    public class GerarNotificacaoCommandValidator : AbstractValidator<GerarNotificacaoCommand>
    {
        public GerarNotificacaoCommandValidator()
        {
            RuleFor(c => c.Ano)
               .NotEmpty()
               .WithMessage("O ano deve ser informado para geração da notificação.");

            RuleFor(c => c.Categoria)
               .NotEmpty()
               .WithMessage("A categoria da notificação deve ser informada para geração da notificação.");

            RuleFor(c => c.DreId)
               .NotEmpty()
               .WithMessage("O código da DRE deve ser informado para geração da notificação.");

            RuleFor(c => c.Mensagem)
              .NotEmpty()
              .WithMessage("A mensagem deve ser informada para geração da notificação.");

            RuleFor(c => c.UsuarioId)
              .NotEmpty()
              .WithMessage("O identificador do usuario deve ser informado para geração da notificação.");

            RuleFor(c => c.Tipo)
               .NotEmpty()
               .WithMessage("O tipo da notificação deve ser informada para geração da notificação.");

            RuleFor(c => c.Titulo)
               .NotEmpty()
               .WithMessage("O título da notificação deve ser informado para geração da notificação.");

            RuleFor(c => c.TurmaId)
               .NotEmpty()
               .WithMessage("O código da turma deve ser informado para geração da notificação.");

            RuleFor(c => c.UeId)
               .NotEmpty()
               .WithMessage("O código da UE deve ser informado para geração da notificação.");
        }
    }
}
