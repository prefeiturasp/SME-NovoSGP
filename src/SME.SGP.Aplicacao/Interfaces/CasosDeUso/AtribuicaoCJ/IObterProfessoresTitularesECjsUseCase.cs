using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterProfessoresTitularesECjsUseCase
    {
        Task<AtribuicaoCJTitularesRetornoDto> Executar(string ueId, string turmaId,
                    string professorRf, Modalidade modalidadeId, int anoLetivo);
    }
}
