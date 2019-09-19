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
        private readonly IRepositorioUsuario repositorioUsuario;

        public ConsultasNotificacao(IRepositorioNotificacao repositorioNotificacao, IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public IEnumerable<NotificacaoBasicaDto> Listar(NotificacaoFiltroDto filtroNotificacaoDto)
        {
            var retorno = repositorioNotificacao.ObterPorDreOuEscolaOuStatusOuTurmoOuUsuarioOuTipoOuCategoriaOuTitulo(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.UeId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioRf,
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria, filtroNotificacaoDto.Titulo, filtroNotificacaoDto.Codigo, filtroNotificacaoDto.AnoLetivo);

            return from r in retorno
                   select new NotificacaoBasicaDto()
                   {
                       Id = r.Id,
                       Titulo = r.Titulo,
                       Data = r.CriadoEm.ToString(),
                       DescricaoStatus = r.Status.ToString(),
                       Status = r.Status,
                       Categoria = r.Categoria,
                       Tipo = r.Tipo.GetAttribute<DisplayAttribute>().Name,
                       Codigo = r.Codigo,
                       PodeRemover = r.PodeRemover,
                       PodeMarcarComoLida = r.DeveMarcarComoLido
                   };
        }

        public NotificacaoDetalheDto Obter(long notificacaoId)
        {
            var notificacao = repositorioNotificacao.ObterPorId(notificacaoId);

            if (notificacao == null)
                throw new NegocioException($"Notificação de Id: '{notificacaoId}' não localizada.");

            if (notificacao.Status != NotificacaoStatus.Lida && notificacao.MarcarComoLidaAoObterDetalhe())
                repositorioNotificacao.Salvar(notificacao);

            var retorno = MapearEntidadeParaDetalheDto(notificacao);
            if (notificacao.UsuarioId.HasValue)
            {
                notificacao.Usuario = repositorioUsuario.ObterPorId(notificacao.UsuarioId.Value);
                retorno.UsuarioRf = notificacao.Usuario.CodigoRf;
            }

            return retorno;
        }

        public IEnumerable<EnumeradoRetornoDto> ObterCategorias()
        {
            return NotificacaoCategoria.GetValues(typeof(NotificacaoCategoria)).Cast<NotificacaoCategoria>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)v
            }).ToList();
        }

        public IEnumerable<EnumeradoRetornoDto> ObterStatus()
        {
            return NotificacaoCategoria.GetValues(typeof(NotificacaoStatus)).Cast<NotificacaoStatus>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)v
            }).ToList();
        }

        public IEnumerable<EnumeradoRetornoDto> ObterTipos()
        {
            return NotificacaoCategoria.GetValues(typeof(NotificacaoTipo)).Cast<NotificacaoTipo>().Select(v => new EnumeradoRetornoDto
            {
                Descricao = v.GetAttribute<DisplayAttribute>().Name,
                Id = (int)v
            }).ToList();
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
                Titulo = retorno.Titulo,
                MostrarBotaoRemover = retorno.PodeRemover,
                MostrarBotoesDeAprovacao = retorno.DeveAprovar,
                MostrarBotaoMarcarComoLido = retorno.DeveMarcarComoLido,
                CategoriaId = (int)retorno.Categoria,
                TipoId = (int)retorno.Tipo,
                StatusId = (int)retorno.Status,
                Codigo = retorno.Codigo,
                Observacao = retorno.Observacao
            };
        }
    }
}