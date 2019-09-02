using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPlanoAnual
    {
        Task Migrar(MigrarPlanoAnualDto migrarPlanoAnualDto);

        void Salvar(PlanoAnualDto planoAnualDto);
    }
}