using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosTipoAtividadeAvaliativa : IComandosTipoAtividadeAvaliativa
    {
        private readonly IRepositorioTipoAtividadeAvaliativa repositorioTipoAtividadeAvaliativa;

        public ComandosTipoAtividadeAvaliativa(IRepositorioTipoAtividadeAvaliativa repositorioTipoAtividadeAvaliativa)
        {
            this.repositorioTipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioTipoAtividadeAvaliativa));
        }

        public async Task Alterar(TipoAtividadeAvaliativaDto dto, long idTipoAtividadeAvaliativa)
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

        public async Task Inserir(TipoAtividadeAvaliativaDto dto)
        {
            var atividadeAvaliativa = MapearDtoParaEntidade(dto, 0L);
            await repositorioTipoAtividadeAvaliativa.SalvarAsync(atividadeAvaliativa);
        }

        private TipoAtividadeAvaliativa MapearDtoParaEntidade(TipoAtividadeAvaliativaDto dto, long id)
        {
            TipoAtividadeAvaliativa tipoAtividadeAvaliativa = new TipoAtividadeAvaliativa();
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