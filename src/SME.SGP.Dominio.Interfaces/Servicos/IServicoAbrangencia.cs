using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoAbrangencia
    {
        void RemoverAbrangencias(long[] ids);

        Task Salvar(string login, Guid perfil, bool ehLogin);

        void SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);

        Task SincronizarEstruturaInstitucionalVigenteCompleta();

        bool DreEstaNaAbrangencia(string login, Guid perfilId, string codigoDre);

        bool UeEstaNaAbrangecia(string login, Guid perfilId, string codigoDre, string codigoUE);
    }
}