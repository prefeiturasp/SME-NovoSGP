using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosFechamento
    {
        Task Salvar(FechamentoDto fechamentoDto);
    }
}