using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAulaPrevista
    {
        Task<AulasPrevistasDadasAuditoriaDto> Inserir(AulaPrevistaDto dto);

        Task<string> Alterar(AulaPrevistaDto dto, long id);
    }
}
