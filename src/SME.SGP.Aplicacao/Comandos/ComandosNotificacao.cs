using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacao : IComandosNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly IServicoNotificacao servicoNotificacao;

        public ComandosNotificacao(IRepositorioNotificacao repositorioNotificacao, IRepositorioUsuario repositorioUsuario, IServicoNotificacao servicoNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new ArgumentNullException(nameof(repositorioUsuario));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
        }

        public List<AlteracaoStatusNotificacaoDto> Excluir(IList<long> notificacoesId)
        {
            if (notificacoesId == null)
            {
                throw new NegocioException("A lista de notificações deve ser informada.");
            }
            var resultado = new List<AlteracaoStatusNotificacaoDto>();
            foreach (var notificacaoId in notificacoesId)
            {
                try
                {
                    Notificacao notificacao = ObterPorIdENotificarCasoNaoExista(notificacaoId);
                    notificacao.Remover();
                    repositorioNotificacao.Salvar(notificacao);
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Notificação com id: '{notificacaoId}' excluída com sucesso.", true));
                }
                catch (NegocioException nex)
                {
                    resultado.Add(new AlteracaoStatusNotificacaoDto(nex.Message, false));
                }
                catch (Exception)
                {
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Não foi possível excluir a notificação com id: '{notificacaoId}'", false));
                }
            }
            return resultado;
        }

        public List<AlteracaoStatusNotificacaoDto> MarcarComoLida(IList<long> notificacoesId)
        {
            if (notificacoesId == null)
            {
                throw new NegocioException("A lista de notificações deve ser informada.");
            }
            var resultado = new List<AlteracaoStatusNotificacaoDto>();
            foreach (var notificacaoId in notificacoesId)
            {
                try
                {
                    Notificacao notificacao = ObterPorIdENotificarCasoNaoExista(notificacaoId);

                    notificacao.MarcarComoLida();
                    repositorioNotificacao.Salvar(notificacao);
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Notificação com id: '{notificacaoId}' alterada com sucesso.", true));
                }
                catch (NegocioException nex)
                {
                    resultado.Add(new AlteracaoStatusNotificacaoDto(nex.Message, false));
                }
                catch (Exception)
                {
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Não foi possível alterar o status da notificação com id: '{notificacaoId}'", false));
                }
            }
            return resultado;
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

        private Notificacao ObterPorIdENotificarCasoNaoExista(long notificacaoId)
        {
            Notificacao notificacao = repositorioNotificacao.ObterPorId(notificacaoId);
            if (notificacao == null)
            {
                throw new NegocioException($"Notificação com id: '{notificacaoId}' não encontrada.");
            }

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