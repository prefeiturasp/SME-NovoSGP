using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosNotificacao : IComandosNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IServicoNotificacao servicoNotificacao;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IMediator mediator;

        public ComandosNotificacao(IRepositorioNotificacao repositorioNotificacao,
                                   IServicoNotificacao servicoNotificacao,
                                   IServicoUsuario servicoUsuario,
                                   IMediator mediator)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new ArgumentNullException(nameof(repositorioNotificacao));
            this.servicoNotificacao = servicoNotificacao ?? throw new ArgumentNullException(nameof(servicoNotificacao));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<List<AlteracaoStatusNotificacaoDto>> Excluir(IList<long> notificacoesId)
        {
            if (notificacoesId.EhNulo())
            {
                throw new NegocioException("A lista de notificações deve ser informada.");
            }
            var resultado = new List<AlteracaoStatusNotificacaoDto>();
            foreach (var notificacaoId in notificacoesId)
            {
                try
                {
                    Notificacao notificacao = ObterPorIdENotificarCasoNaoExista(notificacaoId);
                    if (notificacao.ValidaExclusao())
                        await mediator.Send(new ExcluirNotificacaoCommand(notificacao));
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

        public async Task<List<AlteracaoStatusNotificacaoDto>> MarcarComoLida(IList<long> notificacoesId)
        {
            if (notificacoesId.EhNulo())
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
                    await mediator.Send(new NotificarLeituraNotificacaoCommand(notificacao));

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

        public async Task Salvar(NotificacaoDto notificacaoDto)
        {
            var notificacao = await MapearParaDominio(notificacaoDto);
            await servicoNotificacao.Salvar(notificacao);
        }

        private async Task<Notificacao> MapearParaDominio(NotificacaoDto notificacaoDto)
        {
            var notificacao = new Notificacao
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
            return await TrataUsuario(notificacao, notificacaoDto.UsuarioRf);
        }

        private Notificacao ObterPorIdENotificarCasoNaoExista(long notificacaoId)
        {
            return repositorioNotificacao.ObterPorId(notificacaoId) ??
                   throw new NegocioException($"Notificação com id: '{notificacaoId}' não encontrada.");
        }

        private async Task<Notificacao> TrataUsuario(Notificacao notificacao, string usuarioRf)
        {
            var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(usuarioRf);
            notificacao.Usuario = usuario;
            notificacao.UsuarioId = usuario.Id;
            return notificacao;
        }
    }
}