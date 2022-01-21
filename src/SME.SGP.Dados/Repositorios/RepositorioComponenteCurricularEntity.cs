using Microsoft.Extensions.Configuration;
using SME.SGP.Dados.Repositorios.Base;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioComponenteCurricularEntity : RepositorioBaseEntity<ComponenteCurricularSgp>, IRepositorioComponenteCurricularEntity
    {
        public RepositorioComponenteCurricularEntity(IConfiguration configuration): base(configuration)
        {}
    }    
}

