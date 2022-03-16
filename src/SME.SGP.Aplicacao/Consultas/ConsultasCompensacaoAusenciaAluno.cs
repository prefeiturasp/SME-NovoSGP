using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusenciaAluno : IConsultasCompensacaoAusenciaAluno
    {
        private readonly IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAluno;

        public ConsultasCompensacaoAusenciaAluno(IRepositorioCompensacaoAusenciaAlunoConsulta repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }

        public async Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId)
            => await repositorioCompensacaoAusenciaAluno.ObterPorCompensacao(compensacaoId);
    }
}
