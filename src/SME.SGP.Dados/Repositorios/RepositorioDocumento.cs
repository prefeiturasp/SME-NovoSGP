using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioDocumento : RepositorioBase<Documento>, IRepositorioDocumento
    {
        public RepositorioDocumento(ISgpContext conexao) : base(conexao)
        {
        }

}