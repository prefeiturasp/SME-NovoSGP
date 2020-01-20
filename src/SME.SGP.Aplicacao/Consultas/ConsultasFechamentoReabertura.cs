using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoReabertura : ConsultasBase, IConsultasFechamentoReabertura
    {
        private readonly IConsultasTipoCalendario consultasTipoCalendario;
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ConsultasFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
            IContextoAplicacao contextoAplicacao, IConsultasTipoCalendario consultasTipoCalendario) : base(contextoAplicacao)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.consultasTipoCalendario = consultasTipoCalendario ?? throw new System.ArgumentNullException(nameof(consultasTipoCalendario));
        }

        public async Task<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>> Listar(long tipoCalendarioId, long? dreId, long? ueId)
        {
            var listaEntidades = await repositorioFechamentoReabertura.ListarPaginado(tipoCalendarioId, dreId, ueId, Paginacao);

            return MapearListaEntidadeParaDto(listaEntidades);
        }

        private IEnumerable<FechamentoReaberturaListagemDto> MapearEntidadeParaDto(IEnumerable<FechamentoReabertura> items)
        {
            if (items.Any())
            {
                var tipoCalendario = consultasTipoCalendario.BuscarPorId(items.FirstOrDefault().TipoCalendarioId);
                if (tipoCalendario == null)
                    throw new NegocioException("Não foi possível localizar o tipo de calendário.");

                foreach (var item in items)
                {
                    yield return new FechamentoReaberturaListagemDto()
                    {
                        Bimestres = item.Bimestres.Select(a => a.Bimestre).ToArray(),
                        DataInicio = item.Inicio,
                        DataFim = item.Fim,
                        Descricao = item.Descricao,
                        Id = item.Id,
                        BimestresQuantidadeTotal = tipoCalendario.QuantidadeDeBimestres()
                    };
                }
            }
        }

        private PaginacaoResultadoDto<FechamentoReaberturaListagemDto> MapearListaEntidadeParaDto(PaginacaoResultadoDto<FechamentoReabertura> listaEntidades)
        {
            var retorno = new PaginacaoResultadoDto<FechamentoReaberturaListagemDto>();

            retorno.TotalPaginas = listaEntidades.TotalPaginas;
            retorno.TotalRegistros = listaEntidades.TotalRegistros;

            retorno.Items = MapearEntidadeParaDto(listaEntidades.Items);

            return retorno;
        }
    }
}