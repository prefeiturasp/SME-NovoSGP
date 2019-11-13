using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGradeDisciplina : RepositorioBase<GradeDisciplina>, IRepositorioGradeDisciplina
    {
        public RepositorioGradeDisciplina(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
