using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSupervisorEscolaDre : RepositorioBase<SupervisorEscolaDre>
    {
        public RepositorioSupervisorEscolaDre(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
