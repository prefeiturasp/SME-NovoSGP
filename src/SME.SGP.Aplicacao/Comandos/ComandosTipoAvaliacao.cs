using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoAavaliacao : IComandosTipoAvaliacao
    {
        private readonly IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa;
        private readonly IRepositorioTipoAvaliacao repositorioTipoAvaliacao;

        public ComandosTipoAavaliacao(IRepositorioTipoAvaliacao repositorioTipoAtividadeAvaliativa, IRepositorioAtividadeAvaliativa repositorioAtividadeAvaliativa)
        {
            this.repositorioTipoAvaliacao = repositorioTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioTipoAtividadeAvaliativa));
            this.repositorioAtividadeAvaliativa = repositorioAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioAtividadeAvaliativa));
        }

        public async Task Alterar(TipoAvaliacaoDto dto, long id)
        {
            if (await VerificarSeExisteTipoAvaliacaoPorNome(dto.Nome, dto.Descricao, dto.Situacao, id))
                throw new NegocioException("Já existe tipo de avaliação com esse nome");
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, id);
            await repositorioTipoAvaliacao.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Excluir(long[] ids)
        {
            List<long> idsComErroAoExcluir = new List<long>();

            foreach (var idTipoAtividadeAvaliativa in ids)
            {
                try
                {
                    var tipoAtividadeAvaliativa = repositorioTipoAvaliacao.ObterPorId(idTipoAtividadeAvaliativa);
                    if (tipoAtividadeAvaliativa is null)
                        throw new NegocioException("Não foi possível localizar esta avaliação.");
                    if (await VerificarSeExisteAtividadeVinculada(idTipoAtividadeAvaliativa))
                        throw new NegocioException("Já existe atividade avaliativa vinculada ao tipo de avaliação " + tipoAtividadeAvaliativa.Nome);
                    tipoAtividadeAvaliativa.Excluir();

                    await repositorioTipoAvaliacao.SalvarAsync(tipoAtividadeAvaliativa);
                }
                catch (Exception)
                {
                    idsComErroAoExcluir.Add(idTipoAtividadeAvaliativa);
                }
            }

            if (idsComErroAoExcluir.Any())
                throw new NegocioException($"Não foi possível excluir os tipos de avaliação de ids {string.Join(",", idsComErroAoExcluir)}");
        }

        public async Task Inserir(TipoAvaliacaoDto dto)
        {
            if (await VerificarSeExisteTipoAvaliacaoPorNome(dto.Nome, dto.Descricao, dto.Situacao, 0L))
                throw new NegocioException("Já existe tipo de avaliação com esse nome");
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L);
            await repositorioTipoAvaliacao.SalvarAsync(atividadeAvaliativa);
        }

        private TipoAvaliacao MapearDtoParaEntidade(TipoAvaliacaoDto dto, long id)
        {
            TipoAvaliacao tipoAtividadeAvaliativa = new TipoAvaliacao();
            if (id > 0L)
            {
                tipoAtividadeAvaliativa = repositorioTipoAvaliacao.ObterPorId(id);
            }
            tipoAtividadeAvaliativa.Nome = dto.Nome;
            tipoAtividadeAvaliativa.Descricao = dto.Descricao;
            tipoAtividadeAvaliativa.Situacao = dto.Situacao;
            return tipoAtividadeAvaliativa;
        }

        private async Task<bool> VerificarSeExisteAtividadeVinculada(long tipoAvaliacaoId) => await repositorioAtividadeAvaliativa.VerificarSeJaExistePorTipoAvaliacao(tipoAvaliacaoId);

        private async Task<bool> VerificarSeExisteTipoAvaliacaoPorNome(string nome, string descricao, bool situacao, long id) => await repositorioTipoAvaliacao.VerificarSeJaExistePorNome(nome, descricao, situacao, id);
    }
}