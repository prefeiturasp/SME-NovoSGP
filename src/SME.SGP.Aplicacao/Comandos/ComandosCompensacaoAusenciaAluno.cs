using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ComandosCompensacaoAusenciaAluno : IComandosCompensacaoAusenciaAluno
    {
        private readonly IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno;

        public ComandosCompensacaoAusenciaAluno(IRepositorioCompensacaoAusenciaAluno repositorioCompensacaoAusenciaAluno)
        {
            this.repositorioCompensacaoAusenciaAluno = repositorioCompensacaoAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaAluno));
        }
    }
}
