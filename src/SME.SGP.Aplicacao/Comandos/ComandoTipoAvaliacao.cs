using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoAavaliacao : IComandosTipoAvaliacao
    {
        private readonly IRepositorioTipoAvaliacao repositorioTipoAtividadeAvaliativa;

        public ComandosTipoAavaliacao(IRepositorioTipoAvaliacao repositorioTipoAtividadeAvaliativa)
        {
            this.repositorioTipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioTipoAtividadeAvaliativa));
        }

        public async Task Alterar(TipoAvaliacaoDto dto, long idTipoAtividadeAvaliativa)
        {
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, idTipoAtividadeAvaliativa);
            await repositorioTipoAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        public async Task Excluir(long idTipoAtividadeAvaliativa)
        {
            var tipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa.ObterPorId(idTipoAtividadeAvaliativa);
            if (tipoAtividadeAvaliativa is null)
                throw new NegocioException("Não foi possível localizar esta avaliação.");

            tipoAtividadeAvaliativa.Excluir();

            await repositorioTipoAtividadeAvaliativa.SalvarAsync(tipoAtividadeAvaliativa);
        }

        public async Task Inserir(TipoAvaliacaoDto dto)
        {
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L);
            await repositorioTipoAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        private TipoAvaliacao MapearDtoParaEntidade(TipoAvaliacaoDto dto, long id)
        {
            TipoAvaliacao tipoAtividadeAvaliativa = new TipoAvaliacao();
            if (id > 0L)
            {
                tipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa.ObterPorId(id);
            }
            tipoAtividadeAvaliativa.Nome = dto.Nome;
            tipoAtividadeAvaliativa.Descricao = dto.Descricao;
            tipoAtividadeAvaliativa.Situacao = dto.Situacao;
            return tipoAtividadeAvaliativa;
        }
    }
}