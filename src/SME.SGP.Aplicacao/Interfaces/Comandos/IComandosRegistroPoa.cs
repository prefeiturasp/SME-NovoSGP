using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosRegistroPoa
    {
        void Atualizar(RegistroPoaDto registroPoaDto);

        void Cadastrar(RegistroPoaDto registroPoaDto);

        Task Excluir(long id);
    }
}