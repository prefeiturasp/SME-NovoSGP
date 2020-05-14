using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusenciaDisciplinaRegencia: IConsultasCompensacaoAusenciaDisciplinaRegencia
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public ConsultasCompensacaoAusenciaDisciplinaRegencia(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
        }

        public async Task<IEnumerable<CompensacaoAusenciaDisciplinaRegencia>> ObterPorCompensacao(long compensacaoId)
            => await repositorioCompensacaoAusenciaDisciplinaRegencia.ObterPorCompensacao(compensacaoId);
    }
}
