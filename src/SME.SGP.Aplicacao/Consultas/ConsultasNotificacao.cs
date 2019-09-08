using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotificacao : IConsultasNotificacao
    {
        private readonly IRepositorioNotificacao repositorioNotificacao;

        public ConsultasNotificacao(IRepositorioNotificacao repositorioNotificacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
        }

        public IEnumerable<NotificacaoBasicaDto> Listar(NotificacaoFiltroDto filtroNotificacaoDto)
        {
            var retorno = repositorioNotificacao.ObterPorDreOuEscolaOuStatusOuTurmoOuUsuarioOuTipo(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.EscolaId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioId,
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria);

            return from r in retorno
                   select new NotificacaoBasicaDto()
                   {
                       Id = r.Id,
                       Titulo = r.Titulo,
                       Data = r.CriadoEm.ToString(),
                       Status = r.Status.ToString(),
                       Tipo = r.Tipo.GetAttribute<DisplayAttribute>().Name
                   };
        }

        public NotificacaoDetalheDto Obter(long notificacaoId)
        {
            var notificacao = repositorioNotificacao.ObterPorId(notificacaoId);

            if (notificacao == null)
                throw new NegocioException($"Notificação de Id: '{notificacaoId}' não localizada.");

            if (notificacao.Status != NotificacaoStatus.Lida && notificacao.MarcarComoLidaAoObterDetalhe())
                repositorioNotificacao.Salvar(notificacao);

            var detalheDto = MapearEntidadeParaDetalheDto(notificacao);
            detalheDto.MostrarBotaoMarcarComoLido = notificacao.DeveMarcarComoLido;
            detalheDto.MostrarBotoesDeAprovacao = notificacao.DeveAprovar;

            return detalheDto;
        }

        private static NotificacaoDetalheDto MapearEntidadeParaDetalheDto(Dominio.Notificacao retorno)
        {
            return new NotificacaoDetalheDto()
            {
                AlteradoEm = retorno.AlteradoEm.ToString(),
                AlteradoPor = retorno.AlteradoPor,
                CriadoEm = retorno.CriadoEm.ToString(),
                CriadoPor = retorno.CriadoPor,
                Id = retorno.Id,
                Mensagem = retorno.Mensagem,
                Situacao = retorno.Status.ToString(),
                Tipo = retorno.Tipo.GetAttribute<DisplayAttribute>().Name,
                Titulo = retorno.Titulo
            };
        }
    }
}