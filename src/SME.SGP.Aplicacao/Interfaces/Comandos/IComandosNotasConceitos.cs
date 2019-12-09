using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosNotasConceitos
    {
        Task Salvar(NotaConceitoListaDto notaConceitoLista);
    }
}