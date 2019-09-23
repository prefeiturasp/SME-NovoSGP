using System;

namespace SME.SGP.Aplicacao.Servicos
{
    public interface IServicoTokenJwt
    {
        string GerarToken(string usuarioId, string usuarioLogin, string nome, Guid[] perfis);
    }
}