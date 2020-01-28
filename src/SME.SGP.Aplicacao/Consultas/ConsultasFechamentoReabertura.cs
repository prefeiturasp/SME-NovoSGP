using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasFechamentoReabertura : ConsultasBase, IConsultasFechamentoReabertura
    {
        private readonly IRepositorioFechamentoReabertura repositorioFechamentoReabertura;

        public ConsultasFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
        }

        public async Task<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>> Listar(long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            var listaEntidades = await repositorioFechamentoReabertura.ListarPaginado(tipoCalendarioId, dreCodigo, ueCodigo, Paginacao);

            return MapearListaEntidadeParaDto(listaEntidades);
        }

        public FechamentoReaberturaRetornoDto ObterPorId(long id)
        {
            var fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(id);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar esta reabertura de fechamento.");

            return TransformaEntidadeEmDto(fechamentoReabertura);
        }

        private static FechamentoReaberturaListagemDto TransformaEntidadeEmDtoListagem(FechamentoReabertura item)
        {
            return new FechamentoReaberturaListagemDto()
            {
                Bimestres = item.ObterBimestresSelecionados(),
                DataInicio = item.Inicio,
                DataFim = item.Fim,
                Descricao = item.Descricao,
                Id = item.Id,
                BimestresQuantidadeTotal = item.TipoCalendario.QuantidadeDeBimestres()
            };
        }

        private IEnumerable<FechamentoReaberturaListagemDto> MapearListaEntidadeParaDto(IEnumerable<FechamentoReabertura> items)
        {
            if (items.Any())
            {
                foreach (var item in items)
                {
                    yield return TransformaEntidadeEmDtoListagem(item);
                }
            }
        }

        private PaginacaoResultadoDto<FechamentoReaberturaListagemDto> MapearListaEntidadeParaDto(PaginacaoResultadoDto<FechamentoReabertura> listaEntidades)
        {
            var retorno = new PaginacaoResultadoDto<FechamentoReaberturaListagemDto>();

            retorno.TotalPaginas = listaEntidades.TotalPaginas;
            retorno.TotalRegistros = listaEntidades.TotalRegistros;

            retorno.Items = MapearListaEntidadeParaDto(listaEntidades.Items);

            return retorno;
        }

        private FechamentoReaberturaRetornoDto TransformaEntidadeEmDto(FechamentoReabertura fechamentoReabertura)
        {
            return new FechamentoReaberturaRetornoDto()
            {
                Bimestres = fechamentoReabertura.ObterBimestresSelecionados(),
                DataInicio = fechamentoReabertura.Inicio,
                DataFim = fechamentoReabertura.Fim,
                Descricao = fechamentoReabertura.Descricao,
                Id = fechamentoReabertura.Id,
                BimestresQuantidadeTotal = fechamentoReabertura.TipoCalendario.QuantidadeDeBimestres(),
                DreCodigo = fechamentoReabertura.Dre?.CodigoDre,
                UeCodigo = fechamentoReabertura.Ue?.CodigoUe,
                TipoCalendarioId = fechamentoReabertura.TipoCalendarioId,
                CriadoEm = fechamentoReabertura.CriadoEm,
                AlteradoEm = fechamentoReabertura.AlteradoEm ?? DateTime.MinValue,
                CriadoPor = fechamentoReabertura.CriadoPor,
                AlteradoPor = fechamentoReabertura.AlteradoPor,
                AlteradoRF = fechamentoReabertura.AlteradoRF,
                CriadoRF = fechamentoReabertura.CriadoRF
            };
        }
    }
}