using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoAbrangencia
    {
        Task Salvar(string login, Guid perfil, bool ehLogin);
    }
}