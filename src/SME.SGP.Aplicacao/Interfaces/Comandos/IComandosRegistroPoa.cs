using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosRegistroPoa
    {
        Task Atualizar(RegistroPoaDto registroPoaDto);

        Task Cadastrar(RegistroPoaDto registroPoaDto);

        Task Excluir(long id);
    }
}