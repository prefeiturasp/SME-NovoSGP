using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaEncaminhamentoAEE : RepositorioBase<RespostaEncaminhamentoAEE>, IRepositorioRespostaEncaminhamentoAEE
    {
        public RepositorioRespostaEncaminhamentoAEE(ISgpContext database) : base(database)
        {
        }
    }
}
