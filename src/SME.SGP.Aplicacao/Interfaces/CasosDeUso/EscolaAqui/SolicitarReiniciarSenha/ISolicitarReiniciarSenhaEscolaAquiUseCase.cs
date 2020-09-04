using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha
{
    public interface ISolicitarReiniciarSenhaEscolaAquiUseCase
    {
        Task<RespostaSolicitarReiniciarSenhaEscolaAquiDto> Executar(string cpf);
    }
}
