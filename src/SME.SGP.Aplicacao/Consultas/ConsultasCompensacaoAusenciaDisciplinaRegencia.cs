using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ConsultasCompensacaoAusenciaDisciplinaRegencia: IConsultasCompensacaoAusenciaDisciplinaRegencia
    {
        private readonly IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia;

        public ConsultasCompensacaoAusenciaDisciplinaRegencia(IRepositorioCompensacaoAusenciaDisciplinaRegencia repositorioCompensacaoAusenciaDisciplinaRegencia)
        {
            this.repositorioCompensacaoAusenciaDisciplinaRegencia = repositorioCompensacaoAusenciaDisciplinaRegencia ?? throw new ArgumentNullException(nameof(repositorioCompensacaoAusenciaDisciplinaRegencia));
        }
    }
}
