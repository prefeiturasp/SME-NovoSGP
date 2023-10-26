﻿using SME.SGP.Dominio;
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
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;

        public ConsultaTipoAvaliacao(IRepositorioTipoAvaliacao repositorioTipoAvaliacao, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa
            , IContextoAplicacao contextAplicacao) : base(contextAplicacao)
        {
            this.repositorioTipoAvaliacao = repositorioTipoAvaliacao ?? throw new System.ArgumentNullException(nameof(repositorioTipoAvaliacao));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task<PaginacaoResultadoDto<TipoAvaliacaoCompletaDto>> ListarPaginado(string nome, string descricao, bool? situacao)
        {
            return MapearParaDtoComPaginacao(await repositorioTipoAvaliacao.ListarPaginado(nome, descricao, situacao, Paginacao));
        }

        public async Task<TipoAvaliacaoCompletaDto> ObterPorId(long id)
        {
            var tipoAvaliacao = await repositorioTipoAvaliacao.ObterPorIdAsync(id);
            var possuAvaliacaoVinculada = await VerificarSeExisteAtividadeVinculada(id);

            return MapearParaDto(tipoAvaliacao, possuAvaliacaoVinculada);
        }

        public async Task<TipoAvaliacaoCompletaDto> ObterTipoAvaliacaoBimestral()
            => MapearParaDto(await repositorioTipoAvaliacao.ObterTipoAvaliacaoBimestral(), false);

        private TipoAvaliacaoCompletaDto MapearParaDto(TipoAvaliacao tipoAvaliacao, bool? possuiAvaliacaoVinculada)
        {
            return tipoAvaliacao.EhNulo() ? null : new TipoAvaliacaoCompletaDto
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
                CriadoRF = tipoAvaliacao.CriadoRF,
                possuiAvaliacao = possuiAvaliacaoVinculada,
                AvaliacoesNecessariasPorBimestre = tipoAvaliacao.AvaliacoesNecessariasPorBimestre,
            };
        }

        private PaginacaoResultadoDto<TipoAvaliacaoCompletaDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<TipoAvaliacao> tipoAvaliacaoPaginado)
        {
            if (tipoAvaliacaoPaginado.EhNulo())
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
            return items?.Select(c => MapearParaDto(c, false));
        }

        private async Task<bool> VerificarSeExisteAtividadeVinculada(long tipoAvaliacaoId) => await repositorioAtividadeAvaliativa.VerificarSeJaExistePorTipoAvaliacao(tipoAvaliacaoId);
    }
}