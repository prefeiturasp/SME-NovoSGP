using SME.SGP.Dominio;
using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAbrangencia
    {
        void RemoverAbrangencias(long[] ids);

        void RemoverAbrangenciasHistoricas(long[] ids);

        void RemoverAbrangenciasHistoricasIncorretas(string login, List<Guid> perfis);

        Task<IEnumerable<AbrangenciaHistoricaDto>> ObterAbrangenciaHistorica(string login);

        Task Salvar(string login, Guid perfil, bool ehLogin);

        void SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);

        Task SincronizarEstruturaInstitucionalVigenteCompleta();

        bool DreEstaNaAbrangencia(string login, Guid perfilId, string codigoDre);

        bool UeEstaNaAbrangecia(string login, Guid perfilId, string codigoDre, string codigoUE);
        Task<bool> SincronizarAbrangenciaHistorica(int anoLetivo, string professorRf);
    }
}