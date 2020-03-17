using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoAnual
    {
        Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto);

        Task Salvar(PlanoAnualDto planoAnualDto);
    }
}