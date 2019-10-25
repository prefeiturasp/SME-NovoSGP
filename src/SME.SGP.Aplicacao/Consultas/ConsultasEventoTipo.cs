using Microsoft.AspNetCore.Http;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasEventoTipo : ConsultasBase, IConsultasEventoTipo
    {
        private readonly IRepositorioEventoTipo repositorioEventoTipo;

        public ConsultasEventoTipo(IRepositorioEventoTipo repositorioEventoTipo, IHttpContextAccessor httpContext) : base(httpContext)
        {
            this.repositorioEventoTipo = repositorioEventoTipo ?? throw new ArgumentNullException(nameof(repositorioEventoTipo));
        }

        public async Task<PaginacaoResultadoDto<EventoTipoDto>> Listar(FiltroEventoTipoDto Filtro)
        {
            var retornoQueryPaginada = await repositorioEventoTipo.ListarTipos(Filtro.LocalOcorrencia, Filtro.Letivo, Filtro.Descricao, Paginacao);

            var retornoConsultaPaginada = new PaginacaoResultadoDto<EventoTipoDto>
            {
                TotalPaginas = retornoQueryPaginada.TotalPaginas,
                TotalRegistros = retornoQueryPaginada.TotalRegistros
            };

            bool nenhumItemEncontrado = retornoQueryPaginada.Items == null ||
                !retornoQueryPaginada.Items.Any() ||
                retornoQueryPaginada.Items.ElementAt(0).Id == 0;

            retornoConsultaPaginada.Items = nenhumItemEncontrado
                ? null
                : retornoQueryPaginada.Items.Select(x => EntidadeParaDto(x)).ToList();

            return retornoConsultaPaginada;
        }

        public EventoTipoDto ObterPorId(long id)
        {
            var entidade = repositorioEventoTipo.ObterPorId(id);

            if (entidade == null || entidade.Id == 0) return null;

            if (entidade.Excluido) return null;

            return EntidadeParaDto(entidade);
        }

        private EventoTipoDto EntidadeParaDto(EventoTipo eventoTipo)
        {
            if (eventoTipo == null || eventoTipo.Id == 0)
                return null;

            return new EventoTipoDto
            {
                Descricao = eventoTipo.Descricao,
                Id = eventoTipo.Id,
                Concomitancia = eventoTipo.Concomitancia,
                Dependencia = eventoTipo.Dependencia,
                Letivo = eventoTipo.Letivo,
                Ativo = eventoTipo.Ativo,
                LocalOcorrencia = eventoTipo.LocalOcorrencia,
                TipoData = eventoTipo.TipoData,
                AlteradoEm = eventoTipo.AlteradoEm,
                AlteradoPor = eventoTipo.AlteradoPor,
                AlteradoRF = eventoTipo.AlteradoRF,
                CriadoEm = eventoTipo.CriadoEm,
                CriadoPor = eventoTipo.CriadoPor,
                CriadoRF = eventoTipo.CriadoRF
            };
        }
    }
}