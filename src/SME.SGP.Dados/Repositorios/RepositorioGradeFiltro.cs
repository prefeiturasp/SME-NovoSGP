using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGradeFiltro : RepositorioBase<GradeFiltro>, IRepositorioGradeFiltro
    {
        public RepositorioGradeFiltro(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
