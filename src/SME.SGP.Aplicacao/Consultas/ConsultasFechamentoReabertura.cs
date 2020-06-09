﻿using SME.SGP.Dominio;
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
        private readonly IRepositorioFechamentoReaberturaBimestre repositorioFechamentoReaberturaBimestre;

        public ConsultasFechamentoReabertura(IRepositorioFechamentoReabertura repositorioFechamentoReabertura,
            IRepositorioFechamentoReaberturaBimestre repositorioFechamentoReaberturaBimestre,
            IContextoAplicacao contextoAplicacao) : base(contextoAplicacao)
        {
            this.repositorioFechamentoReabertura = repositorioFechamentoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReabertura));
            this.repositorioFechamentoReaberturaBimestre = repositorioFechamentoReaberturaBimestre ?? throw new System.ArgumentNullException(nameof(repositorioFechamentoReaberturaBimestre));
        }

        public async Task<PaginacaoResultadoDto<FechamentoReaberturaListagemDto>> Listar(long tipoCalendarioId, string dreCodigo, string ueCodigo)
        {
            var listaEntidades = await repositorioFechamentoReabertura.ListarPaginado(tipoCalendarioId, dreCodigo, ueCodigo, Paginacao);

            foreach(FechamentoReabertura fechamentoReabertura in listaEntidades.Items)
            {
                var bimestres = await repositorioFechamentoReaberturaBimestre.ObterPorFechamentoReaberturaIdAsync(fechamentoReabertura.Id);
                fechamentoReabertura.AdicionarBimestres(bimestres);
            }
            return MapearListaEntidadeParaDto(listaEntidades);
        }

        public async Task<FechamentoReaberturaRetornoDto> ObterPorId(long id)
        {
            var fechamentoReabertura = repositorioFechamentoReabertura.ObterCompleto(id);
            if (fechamentoReabertura == null)
                throw new NegocioException("Não foi possível localizar esta reabertura de fechamento.");

            return await TransformaEntidadeEmDto(fechamentoReabertura);
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

        private async Task<FechamentoReaberturaRetornoDto> TransformaEntidadeEmDto(FechamentoReabertura fechamentoReabertura)
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
                PossuiFilhos = await FechamentoPossuiFilhos(fechamentoReabertura),
                CriadoEm = fechamentoReabertura.CriadoEm,
                AlteradoEm = fechamentoReabertura.AlteradoEm ?? DateTime.MinValue,
                CriadoPor = fechamentoReabertura.CriadoPor,
                AlteradoPor = fechamentoReabertura.AlteradoPor,
                AlteradoRF = fechamentoReabertura.AlteradoRF,
                CriadoRF = fechamentoReabertura.CriadoRF
            };
        }

        private async Task<bool> FechamentoPossuiFilhos(FechamentoReabertura fechamentoReabertura)
        {
            if (fechamentoReabertura.EhParaSme())
            {
                var fechamentosSME = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendario.Id, null, null, null);

                return fechamentosSME.Any(f => f.EhParaDre() || f.EhParaUe());
            }
            else if (fechamentoReabertura.EhParaDre())
            {
                var fechamentosDre = await repositorioFechamentoReabertura.Listar(fechamentoReabertura.TipoCalendario.Id, fechamentoReabertura.DreId, null, null);

                return (fechamentosDre.Any(f => f.EhParaUe()));
            }

            return false;
        }
    }
}