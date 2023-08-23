using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAbrangencia
    {
        Task RemoverAbrangencias(long[] ids);

        Task RemoverAbrangenciasHistoricas(long[] ids);

        Task RemoverAbrangenciasHistoricasIncorretas(string login, List<Guid> perfis);

        Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistorica(string login);

        Task Salvar(string login, Guid perfil, bool ehLogin);

        Task SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);

        Task SincronizarEstruturaInstitucionalVigenteCompleta();

        Task<bool> DreEstaNaAbrangencia(string login, Guid perfilId, string codigoDre);

        Task<bool> UeEstaNaAbrangecia(string login, Guid perfilId, string codigoDre, string codigoUE);
        Task<bool> SincronizarAbrangenciaHistorica(int anoLetivo, string professorRf, long turmaId = 0);
        Task<IEnumerable<string>> ObterLoginsAbrangenciaUePorPerfil(long ueId, Guid perfil, bool historica = false);
    }
}