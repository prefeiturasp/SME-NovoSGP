using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoAbrangencia
    {
        void RemoverAbrangencias(long[] ids);

        Task Salvar(string login, Guid perfil, bool ehLogin);

        void SalvarAbrangencias(IEnumerable<Abrangencia> abrangencias, string login);
    }
}