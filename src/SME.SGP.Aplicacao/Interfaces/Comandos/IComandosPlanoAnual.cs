using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoAnual
    {
        Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto);

        Task<IEnumerable<PlanoAnualCompletoDto>> Salvar(PlanoAnualDto planoAnualDto);
    }
}