using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGrade : RepositorioBase<Grade>, IRepositorioGrade
    {
        public RepositorioGrade(ISgpContext conexao) : base(conexao)
        {
        }

    }
}
