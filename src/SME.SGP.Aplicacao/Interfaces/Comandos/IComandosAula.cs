using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAula
    {
        Task Alterar(AulaDto dto, long id);

        void Excluir(long id);

        Task<string> Inserir(AulaDto dto);
    }
}