using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<string> Copiar(CompensacaoAusenciaCopiaDto compensacaoCopia)
            => await servicoCompensacaoAusencia.Copiar(compensacaoCopia);

        public async Task Excluir(long[] compensacoesIds)
            => await servicoCompensacaoAusencia.Excluir(compensacoesIds);

        public async Task Inserir(CompensacaoAusenciaDto compensacao)
            => await servicoCompensacaoAusencia.Salvar(0, compensacao);
    }
}
