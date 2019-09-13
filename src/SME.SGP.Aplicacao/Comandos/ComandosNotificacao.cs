using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacao : IComandosNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoNotificacao servicoNotificacao;

        public ComandosNotificacao(IRepositorioNotificacao repositorioNotificacao, IRepositorioUsuario repositorioUsuario, IServicoNotificacao servicoNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.servicoNotificacao = servicoNotificacao ?? throw new System.ArgumentNullException(nameof(servicoNotificacao));
        }

        public void MarcarComoLida(long notificacaoId)
        {
            var notificacao = repositorioNotificacao.ObterPorId(notificacaoId);
            if (notificacao == null)
            {
                throw new NegocioException($"Notificação com id: '{notificacaoId}' não encontrada");
            }
            notificacao.MarcarComoLida();
            repositorioNotificacao.Salvar(notificacao);
        }

        public void Salvar(NotificacaoDto notificacaoDto)
        {
            var notificacao = MapearParaDominio(notificacaoDto);
            servicoNotificacao.GeraNovoCodigo(notificacao);
            repositorioNotificacao.Salvar(notificacao);
        }

        private Notificacao MapearParaDominio(NotificacaoDto notificacaoDto)
        {
            var notificacao = new Notificacao()
            {
                Categoria = notificacaoDto.Categoria,
                DreId = notificacaoDto.DreId,
                UeId = notificacaoDto.UeId,
                Mensagem = notificacaoDto.Mensagem,
                Titulo = notificacaoDto.Titulo,
                Ano = notificacaoDto.Ano,
                TurmaId = notificacaoDto.TurmaId,
                Tipo = notificacaoDto.Tipo
            };

            TrataUsuario(notificacao, notificacaoDto.UsuarioRf);

            return notificacao;
        }

        private void TrataUsuario(Notificacao notificacao, string usuarioRf)
        {
            if (!string.IsNullOrEmpty(usuarioRf))
            {
                Usuario usuario = repositorioUsuario.ObterPorCodigoRf(usuarioRf);
                if (usuario == null)
                {
                    usuario = new Usuario() { CodigoRf = usuarioRf };
                    repositorioUsuario.Salvar(usuario);
                }

                notificacao.Usuario = usuario;
                notificacao.UsuarioId = usuario.Id;
            }
        }
    }
}