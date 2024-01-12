using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasNotificacao : ConsultasBase, IConsultasNotificacao
    {
        private readonly IMediator mediator;
        private readonly IRepositorioTipoRelatorio repositorioTipoRelatorio;

        public ConsultasNotificacao(IContextoAplicacao contextoAplicacao, IMediator mediator,
            IRepositorioTipoRelatorio repositorioTipoRelatorio) : base(contextoAplicacao)
        {
            this.repositorioTipoRelatorio = repositorioTipoRelatorio ?? throw new ArgumentNullException(nameof(repositorioTipoRelatorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<NotificacaoBasicaDto>> Listar(NotificacaoFiltroDto filtroNotificacaoDto)
        {
            var retorno = await mediator.Send(new ObterNotificacoesQuery(filtroNotificacaoDto, this.Paginacao));

            var retornoPaginadoDto = new PaginacaoResultadoDto<NotificacaoBasicaDto>
            {
                TotalRegistros = retorno.TotalRegistros,
                TotalPaginas = retorno.TotalPaginas,

                Items = from r in retorno.Items
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
                        PodeMarcarComoLida = r.Status == NotificacaoStatus.Pendente
                    }
            };

            return retornoPaginadoDto;
        }

        public async Task<IEnumerable<NotificacaoBasicaDto>> ListarPorAnoLetivoRf(int anoLetivo, string usuarioRf, int limite = 5)
        {
            var notificacao = await mediator.Send(new ObterNotificacoesPorAnoLetivoERfQuery(anoLetivo, usuarioRf, limite));

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

        public IEnumerable<EnumeradoRetornoDto> ObterCategorias()
        {
            return EnumExtensao.ListarDto<NotificacaoCategoria>();
        }

        public async Task<NotificacaoBasicaListaDto> ObterNotificacaoBasicaLista(int anoLetivo, string usuarioRf)
        {
            return new NotificacaoBasicaListaDto
            {
                Notificacoes = await ListarPorAnoLetivoRf(anoLetivo, usuarioRf),
                QuantidadeNaoLidas = await QuantidadeNotificacoesNaoLidas(anoLetivo, usuarioRf)
            };
        }

        public IEnumerable<EnumeradoRetornoDto> ObterStatus()
        {
            return EnumExtensao.ListarDto<NotificacaoStatus>();
        }

        public IEnumerable<EnumeradoRetornoDto> ObterTipos()
        {
            return EnumExtensao.ListarDto<NotificacaoTipo>();
        }

        public async Task<int> QuantidadeNotificacoesNaoLidas(int anoLetivo, string usuarioRf)
        {
            return await mediator.Send(new ObterNotificacaoQuantNaoLidasPorAnoLetivoRfAnoLetivoQuery(anoLetivo, usuarioRf));
        }
    }
}