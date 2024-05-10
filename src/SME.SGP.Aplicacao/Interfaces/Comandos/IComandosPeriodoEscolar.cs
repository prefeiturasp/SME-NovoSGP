using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosPeriodoEscolar
    {
        Task Salvar(PeriodoEscolarListaDto periodosDto);
    }
}