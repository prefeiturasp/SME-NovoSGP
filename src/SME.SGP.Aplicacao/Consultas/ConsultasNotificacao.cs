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
            var retorno = repositorioNotificacao.Obter(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.UeId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioRf,
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria, filtroNotificacaoDto.Titulo, filtroNotificacaoDto.Codigo, filtroNotificacaoDto.AnoLetivo);

            return from r in retorno
                   select new NotificacaoBasicaDto()
                   {
                       Id = r.Id,
                       Titulo = r.Titulo,
                       Data = r.CriadoEm,
                       DescricaoStatus = r.Status.GetAttribute<DisplayAttribute>().Name,
                       Status = r.Status,
                       Categoria = r.Categoria,
                       DescricaoCategoria = r.Categoria.GetAttribute<DisplayAttribute>().Name,
                       Tipo = r.Tipo.GetAttribute<DisplayAttribute>().Name,
                       Codigo = r.Codigo,
                       PodeRemover = r.PodeRemover,
                       PodeMarcarComoLida = r.DeveMarcarComoLido
                   };
        }

        public IEnumerable<NotificacaoBasicaDto> ListarPorAnoLetivoRf(int anoLetivo, string usuarioRf, int limite = 5)
        {
            var notificacao = repositorioNotificacao.ObterNotificacoesPorAnoLetivoERf(anoLetivo, usuarioRf, limite);

            return notificacao.Select(x => new NotificacaoBasicaDto
            {
                Id = x.Id,
                Categoria = x.Categoria,
                Codigo = x.Codigo,
                Data = x.CriadoEm,
                DescricaoStatus = x.Mensagem,
                Status = x.Status,
                Tipo = x.Tipo.ToString(),
                Titulo = x.Titulo
            });
        }

        public NotificacaoDetalheDto Obter(long notificacaoId)
        {
            var notificacao = repositorioNotificacao.ObterPorId(notificacaoId);

            if (notificacao == null)
                throw new NegocioException($"Notificação de Id: '{notificacaoId}' não localizada.");

            if (notificacao.Status != NotificacaoStatus.Lida && notificacao.MarcarComoLidaAoObterDetalhe())
                repositorioNotificacao.Salvar(notificacao);

            var retorno = MapearEntidadeParaDetalheDto(notificacao);

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

        public NotificacaoBasicaListaDto ObterNotificacaoBasicaLista(int anoLetivo, string usuarioRf)
        {
            return new NotificacaoBasicaListaDto
            {
                Notificacoes = ListarPorAnoLetivoRf(anoLetivo, usuarioRf),
                QuantidadeNaoLidas = QuantidadeNotificacoesNaoLidas(anoLetivo, usuarioRf)
            };
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

        public int QuantidadeNotificacoesNaoLidas(int anoLetivo, string usuarioRf)
        {
            return repositorioNotificacao.ObterQuantidadeNotificacoesNaoLidasPorAnoLetivoERf(anoLetivo, usuarioRf);
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
                Observacao = retorno.WorkflowAprovacaoNivel == null ? string.Empty : retorno.WorkflowAprovacaoNivel.Observacao
            };
        }
    }
}