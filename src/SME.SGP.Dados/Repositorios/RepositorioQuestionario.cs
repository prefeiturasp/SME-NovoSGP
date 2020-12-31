using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestionario : RepositorioBase<Questionario>, IRepositorioQuestionario
    {
        public RepositorioQuestionario(ISgpContext database) : base(database)
        {
        }
    }
}
