using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAula
    {
        Task Inserir(AulaDto dto);
        Task Alterar(AulaDto dto, long id);
        void Excluir(long id);
    }
}
