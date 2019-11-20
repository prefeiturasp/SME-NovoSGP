using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacao : IComandosNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosNotificacao(IRepositorioNotificacao repositorioNotificacao,
                                   IServicoNotificacao servicoNotificacao,
                                   IServicoUsuario servicoUsuario)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
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
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Notificação com Código: '{notificacao.Codigo}' excluída com sucesso.", true));
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
                    resultado.Add(new AlteracaoStatusNotificacaoDto($"Notificação com Código: '{notificacao.Codigo}' alterada com sucesso.", true));
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
            servicoNotificacao.Salvar(notificacao);
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
                Tipo = notificacaoDto.Tipo,
                Codigo = notificacaoDto.Codigo
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
            var usuario = servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioRf);
            notificacao.Usuario = usuario;
            notificacao.UsuarioId = usuario.Id;
        }
    }
}