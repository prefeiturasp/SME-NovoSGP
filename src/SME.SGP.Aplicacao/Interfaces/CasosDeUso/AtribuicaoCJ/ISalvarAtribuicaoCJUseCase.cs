using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface ISalvarAtribuicaoCJUseCase
    {
        Task Executar(AtribuicaoCJPersistenciaDto persistenciaDto);
    }
}
