using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosCompensacaoAusencia: IComandosCompensacaoAusencia
    {
        private readonly IServicoCompensacaoAusencia servicoCompensacaoAusencia;

        public ComandosCompensacaoAusencia(IServicoCompensacaoAusencia servicoCompensacaoAusencia)
        {
            this.servicoCompensacaoAusencia = servicoCompensacaoAusencia ?? throw new ArgumentNullException(nameof(servicoCompensacaoAusencia));
        }

        public async Task Alterar(long id, CompensacaoAusenciaDto compensacao)
            => await servicoCompensacaoAusencia.Salvar(id, compensacao);

        public async Task Inserir(CompensacaoAusenciaDto compensacao)
            => await servicoCompensacaoAusencia.Salvar(0, compensacao);
    }
}
