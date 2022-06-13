using MediatR;
using SME.SGP.Dados.Repositorios;
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
        private readonly IRepositorioNotificacao repositorioNotificacao;
        private readonly IMediator mediator;
        private readonly IObterDataCriacaoRelatorioUseCase obterDataCriacaoRelatorio;
        private readonly IRepositorioTipoRelatorio repositorioTipoRelatorio;

        public ConsultasNotificacao(IRepositorioNotificacao repositorioNotificacao, IContextoAplicacao contextoAplicacao, IMediator mediator, IObterDataCriacaoRelatorioUseCase obterDataCriacaoRelatorio, IRepositorioTipoRelatorio repositorioTipoRelatorio) : base(contextoAplicacao)
        {
            this.repositorioNotificacao = repositorioNotificacao ?? throw new System.ArgumentNullException(nameof(repositorioNotificacao));
            this.obterDataCriacaoRelatorio = obterDataCriacaoRelatorio ?? throw new System.ArgumentNullException(nameof(obterDataCriacaoRelatorio));
            this.repositorioTipoRelatorio = repositorioTipoRelatorio ?? throw new System.ArgumentNullException(nameof(repositorioTipoRelatorio));
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<NotificacaoBasicaDto>> Listar(NotificacaoFiltroDto filtroNotificacaoDto)
        {
            var retorno = await mediator.Send(new ObterNotificacoesQuery(filtroNotificacaoDto.DreId,
                filtroNotificacaoDto.UeId, (int)filtroNotificacaoDto.Status, filtroNotificacaoDto.TurmaId, filtroNotificacaoDto.UsuarioRf,
                (int)filtroNotificacaoDto.Tipo, (int)filtroNotificacaoDto.Categoria, filtroNotificacaoDto.Titulo, filtroNotificacaoDto.Codigo, filtroNotificacaoDto.AnoLetivo, this.Paginacao));

            var retornoPaginadoDto = new PaginacaoResultadoDto<NotificacaoBasicaDto>();
            retornoPaginadoDto.TotalRegistros = retorno.TotalRegistros;
            retornoPaginadoDto.TotalPaginas = retorno.TotalPaginas;

            retornoPaginadoDto.Items = from r in retorno.Items
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

        public async Task<NotificacaoDetalheDto> Obter(long notificacaoId)
        {
            var notificacao = repositorioNotificacao.ObterPorId(notificacaoId);

            if (notificacao == null)
                throw new NegocioException($"Notificação de Id: '{notificacaoId}' não localizada.");

            if (Regex.Match(notificacao.Mensagem, "<a [^>]*?>").Success && notificacao.Mensagem.Contains("Para visualizar a aula clique"))
            {
                notificacao.Mensagem = Regex.Replace(notificacao.Mensagem, @"Para visualizar a aula clique(\s)?.", "", RegexOptions.IgnoreCase);
                notificacao.Mensagem = Regex.Replace(notificacao.Mensagem, @"<a [^>]*?>(.*?)<\/a>?.", "");
            }
            
            if (notificacao.Status != NotificacaoStatus.Lida && notificacao.MarcarComoLidaAoObterDetalhe())
                repositorioNotificacao.Salvar(notificacao);

            var retorno = await MapearEntidadeParaDetalheDto(notificacao);

            return retorno;
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
                Observacao = retorno.WorkflowAprovacaoNivel == null ? string.Empty : retorno.WorkflowAprovacaoNivel.Observacao
            };
        }

        private static string ObterCodigoArquivo(string mensagem)
        {
            string pattern = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
            Regex rg = new Regex(pattern);
            var codigo = rg.Match(mensagem);
            return codigo.ToString();
        }
        private async Task<bool> VerificarSeArquivoExiste(string codigoArquivo)
        {
            var guidRelatorio = new Guid(codigoArquivo);
            return await obterDataCriacaoRelatorio.Executar(guidRelatorio);
        }
    }
}