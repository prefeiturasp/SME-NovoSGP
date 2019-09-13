using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacao : IComandosNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ComandosNotificacao(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
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
            var noticacao = MapearParaDominio(notificacaoDto);
            repositorioNotificacao.Salvar(noticacao);
        }

        private Notificacao MapearParaDominio(NotificacaoDto notificacaoDto)
        {
            return new Notificacao()
            {
                Categoria = notificacaoDto.Categoria,
                DreId = notificacaoDto.DreId,
                EscolaId = notificacaoDto.EscolaId,
                Mensagem = notificacaoDto.Mensagem,
                PodeRemover = notificacaoDto.PodeRemover,
                UsuarioId = notificacaoDto.UsuarioId,
                Titulo = notificacaoDto.Titulo,
                Ano = notificacaoDto.Ano,
                TurmaId = notificacaoDto.TurmaId,
                Tipo = notificacaoDto.Tipo
            };
        }
    }
}