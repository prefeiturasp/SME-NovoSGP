using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasUsuario
    {
        Task<MeusDadosDto> BuscarMeusDados();

        Task<PerfisApiEolDto> ObterPerfilsUsuarioPorLogin(string login);
    }
}