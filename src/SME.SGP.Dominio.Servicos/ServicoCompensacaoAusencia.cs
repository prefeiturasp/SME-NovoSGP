using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCompensacaoAusencia: IServicoCompensacaoAusencia
    {

        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ServicoCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }
    }
}
