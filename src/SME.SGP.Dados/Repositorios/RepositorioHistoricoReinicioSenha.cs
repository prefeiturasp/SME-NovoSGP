using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioHistoricoReinicioSenha : RepositorioBase<HistoricoReinicioSenha>, IRepositorioHistoricoReinicioSenha
    {
        public RepositorioHistoricoReinicioSenha(ISgpContext database) : base(database)
        {
        }
    }
}
