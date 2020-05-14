using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ComandosCompensacaoAusenciaDisciplinaRegencia : IComandosCompensacaoAusenciaDisciplinaRegencia
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public ComandosCompensacaoAusenciaDisciplinaRegencia(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
        }
    }
}
