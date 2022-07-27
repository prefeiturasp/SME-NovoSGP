using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra.Interface;

namespace SME.SGP.TesteIntegracao.Aula.ServicosFakes
{
    public class ServicoAuditoriaFakeAdministrador: IServicoAuditoria
    {
        public ServicoAuditoriaFakeAdministrador()
        {}

        public async Task<bool> Auditar(Auditoria auditoria)
        {
            if (!auditoria.Administrador.Equals("7924488"))
                throw new NegocioException(MensagemNegocioComuns.A_AUDITORIA_NAO_FOI_REGISTRADA_PELO_ADMINISTRADOR);

            return true;
        }
    }
}
