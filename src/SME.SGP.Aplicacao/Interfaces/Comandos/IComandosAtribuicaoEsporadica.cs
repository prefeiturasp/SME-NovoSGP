using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAtribuicaoEsporadica
    {
        Task Excluir(long idAtribuicaoEsporadica);

        void Salvar(AtribuicaoEsporadicaCompletaDto atruibuicaoEsporadicaCompletaDto);
    }
}