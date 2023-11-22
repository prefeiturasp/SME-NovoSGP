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
            var retorno = await mediator.Send(new ObterNotificacoesQuery(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.UeId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioRf,
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria, filtroNotificacaoDto.Titulo, filtroNotificacaoDto.Codigo, filtroNotificacaoDto.AnoLetivo, this.Paginacao));

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

        private async Task<NotificacaoDetalheDto> MapearEntidadeParaDetalheDto(Notificacao retorno)
        {
            string codigoRelatorio = string.Empty;
            int tipoRelatorio = 0;
            bool relatorioExiste = true;

            if (NotificacaoTipo.Relatorio == retorno.Tipo)
                codigoRelatorio = ObterCodigoArquivo(retorno.Mensagem);

            if (codigoRelatorio.Any())
            {
                tipoRelatorio = await repositorioTipoRelatorio.ObterTipoPorCodigo(codigoRelatorio);
            }

            if (!string.IsNullOrEmpty(codigoRelatorio) && (tipoRelatorio != (int)TipoRelatorio.Itinerancias))
                relatorioExiste = await VerificarSeArquivoExiste(codigoRelatorio);

            return new NotificacaoDetalheDto()
            {
                AlteradoEm = retorno.AlteradoEm.ToString(),
                AlteradoPor = retorno.AlteradoPor,
                CriadoEm = retorno.CriadoEm.ToString(),
                CriadoPor = retorno.CriadoPor,
                Id = retorno.Id,
                Mensagem = relatorioExiste ? retorno.Mensagem : "O arquivo não está mais disponível, solicite a geração do relatório novamente.",
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
                Observacao = retorno.WorkflowAprovacaoNivel.EhNulo() ? string.Empty : retorno.WorkflowAprovacaoNivel.Observacao
            };
        }

        private static string ObterCodigoArquivo(string mensagem)
        {
            string pattern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
            Regex rg = new(pattern);
            var codigo = rg.Match(mensagem);
            return codigo.ToString();
        }

        private async Task<bool> VerificarSeArquivoExiste(string codigoArquivo)
        {
            var guidRelatorio = new Guid(codigoArquivo);
            return await mediator.Send(new VerificarExistenciaRelatorioPorCodigoQuery(guidRelatorio));
        }
    }
}