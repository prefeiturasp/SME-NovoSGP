using System.Threading.Tasks;
using SME.SGP.Dominio; 
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class ServicoAuditoriaFake: IServicoAuditoria
    {
        public ServicoAuditoriaFake()
        {}

        public async Task<bool> Auditar(Auditoria auditoria)
        {
            return true;
        }
    }
}
