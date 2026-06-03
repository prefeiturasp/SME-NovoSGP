using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAbrangenciaCompletaIntegracaoUseCase
    {
        Task<AbrangenciaCompletaRetornoDto> Executar(string login, Guid perfil, bool consideraHistorico, int anoLetivo, int semestre, Modalidade modalidade, string codigoDre, string codigoUe, string codigoTurma, bool includeTurmas);
    }
}
