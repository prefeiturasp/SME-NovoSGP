using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha
{
    public interface ISolicitarReiniciarSenhaUseCase
    {
        Task<RespostaSolicitarReiniciarSenhaDto> Executar(string cpf);
    }
}
