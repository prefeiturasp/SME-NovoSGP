using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultaTipoAvaliacao : ConsultasBase, IConsultaTipoAvaliacao
    {
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;

        public ConsultaTipoAvaliacao(IRepositorioTipoAvaliacao repositorioTipoAvaliacao
            , IContextoAplicacao contextAplicacao) : base(contextAplicacao)

        {
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new System.ArgumentNullException(nameof(repositorioTipoAvaliacao));
        }

        public async Task<PaginacaoResultadoDto<TipoAvaliacaoCompletaDto>> ListarPaginado(string nome)
        {
            return MapearParaDtoComPaginacao(await repositorioTipoAvaliacao.ListarPaginado(nome, Paginacao));
        }

        public TipoAvaliacaoCompletaDto ObterPorId(long id)
        {
            return MapearParaDto(repositorioTipoAvaliacao.ObterPorId(id));
        }

        private TipoAvaliacaoCompletaDto MapearParaDto(TipoAvaliacao tipoAvaliacao)
        {
            return tipoAvaliacao == null ? null : new TipoAvaliacaoCompletaDto
            {
                Id = tipoAvaliacao.Id,
                Nome = tipoAvaliacao.Nome,
                Descricao = tipoAvaliacao.Descricao,
                Situacao = tipoAvaliacao.Situacao,
                AlteradoEm = tipoAvaliacao.AlteradoEm,
                AlteradoPor = tipoAvaliacao.AlteradoPor,
                AlteradoRF = tipoAvaliacao.AlteradoRF,
                CriadoEm = tipoAvaliacao.CriadoEm,
                CriadoPor = tipoAvaliacao.CriadoPor,
                CriadoRF = tipoAvaliacao.CriadoRF
            };
        }

        private PaginacaoResultadoDto<TipoAvaliacaoCompletaDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<TipoAvaliacao> tipoAvaliacaoPaginado)
        {
            if (tipoAvaliacaoPaginado == null)
            {
                tipoAvaliacaoPaginado = new PaginacaoResultadoDto<TipoAvaliacao>();
            }
            return new PaginacaoResultadoDto<TipoAvaliacaoCompletaDto>
            {
                Items = MapearTipoAvaliacaoDtoaParaDto(tipoAvaliacaoPaginado.Items),
                TotalPaginas = tipoAvaliacaoPaginado.TotalPaginas,
                TotalRegistros = tipoAvaliacaoPaginado.TotalRegistros
            };
        }

        private IEnumerable<TipoAvaliacaoCompletaDto> MapearTipoAvaliacaoDtoaParaDto(IEnumerable<TipoAvaliacao> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }
    }
}