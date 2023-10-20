using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCadastroAcessoABAEConsulta : IRepositorioBase<CadastroAcessoABAE>
    {
        Task<bool> ExisteCadastroAcessoABAEPorCpf(string cpf);
    }
}
