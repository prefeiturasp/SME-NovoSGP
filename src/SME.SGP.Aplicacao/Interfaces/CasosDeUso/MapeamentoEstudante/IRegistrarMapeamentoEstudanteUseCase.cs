using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso
{
    public interface IRegistrarMapeamentoEstudanteUseCase
    {
        Task<ResultadoMapeamentoEstudanteDto> Executar(MapeamentoEstudanteDto mapeamentoEstudante);
    }
}
