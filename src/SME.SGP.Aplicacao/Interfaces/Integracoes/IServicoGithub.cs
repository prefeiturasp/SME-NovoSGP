using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoGithub
    {
        Task<string> RecuperarUltimaVersao();

   }
}