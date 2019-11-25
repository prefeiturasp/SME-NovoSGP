using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandoTipoAtividadeAvaliativa : IComandoTipoAtividadeAvaliativa
    {
        private readonly IRepositorioTipoAtividadeAvaliativa repositorioTipoAtividadeAvaliativa;

        public ComandoTipoAtividadeAvaliativa(IRepositorioTipoAtividadeAvaliativa repositorioTipoAtividadeAvaliativa)
        {
            this.repositorioTipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(repositorioTipoAtividadeAvaliativa));
        }

        public async Task Excluir(long idTipoAtividadeAvaliativa)
        {
            var tipoAtividadeAvaliativa = repositorioTipoAtividadeAvaliativa.ObterPorId(idTipoAtividadeAvaliativa);
            if (tipoAtividadeAvaliativa is null)
                throw new NegocioException("Não foi possível localizar esta avaliação.");

            tipoAtividadeAvaliativa.Excluir();

            await repositorioTipoAtividadeAvaliativa.SalvarAsync(tipoAtividadeAvaliativa);
        }
    }
}