using System;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoAbrangencia
    {
        void Salvar(string login, Guid perfil, bool ehLogin);
    }
}