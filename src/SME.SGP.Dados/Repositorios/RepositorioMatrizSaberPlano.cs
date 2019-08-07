using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMatrizSaberPlano : RepositorioBase<MatrizSaberPlano>, IRepositorioMatrizSaberPlano
    {
        public RepositorioMatrizSaberPlano(ISgpContext conexao) : base(conexao)
        {
        }
    }
}