using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoPerfil
    {
        Task<PerfisPorPrioridadeDto> DefinirPerfilPrioritario(IEnumerable<Guid> perfis, Usuario usuario, bool ehCJ);
    }
}