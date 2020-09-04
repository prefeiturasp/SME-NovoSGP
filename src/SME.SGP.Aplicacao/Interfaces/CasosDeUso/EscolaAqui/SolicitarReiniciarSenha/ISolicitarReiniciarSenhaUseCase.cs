using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha
{
    public interface ISolicitarReiniciarSenhaUseCase
    {
        Task Executar(string cpf);
    }
}
