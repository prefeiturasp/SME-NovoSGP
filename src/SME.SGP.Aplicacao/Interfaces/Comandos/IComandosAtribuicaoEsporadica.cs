using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAtribuicaoEsporadica
    {
        Task Salvar(AtribuicaoEsporadicaDto atruibuicaoEsporadicaDto);
    }
}