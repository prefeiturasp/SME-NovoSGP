using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoCompensacaoAusencia: IServicoCompensacaoAusencia
    {

        private readonly IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia;

        public ServicoCompensacaoAusencia(IRepositorioCompensacaoAusencia repositorioCompensacaoAusencia)
        {
            this.repositorioCompensacaoAusencia = repositorioCompensacaoAusencia ?? throw new System.ArgumentNullException(nameof(repositorioCompensacaoAusencia));
        }

        public Task Salvar(long id, CompensacaoAusenciaDto compensacao)
        {
            throw new NotImplementedException();
        }
    }
}
