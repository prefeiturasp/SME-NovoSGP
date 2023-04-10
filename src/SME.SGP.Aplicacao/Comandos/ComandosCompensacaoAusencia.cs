using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosCompensacaoAusencia: IComandosCompensacaoAusencia
    {
        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;
        private readonly IServicoCompensacaoAusencia servicoCompensacaoAusencia;

        public ComandosCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia,
                                           IServicoCompensacaoAusencia servicoCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusencia));
            this.servicoCompensacaoAusencia = servicoCompensacaoAusencia ?? throw new ArgumentNullException(nameof(servicoCompensacaoAusencia));
        }

        public async Task Alterar(long id, CompensacaoAusenciaDto compensacao)
            => await servicoCompensacaoAusencia.Salvar(id, compensacao);

        public async Task Inserir(CompensacaoAusenciaDto compensacao)
            => await servicoCompensacaoAusencia.Salvar(0, compensacao);
    }
}
