using SME.SGP.Infra;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoAnual
    {
        void Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto);

        Task<IEnumerable<PlanoAnualCompletoDto>> Salvar(PlanoAnualDto planoAnualDto);
    }
}