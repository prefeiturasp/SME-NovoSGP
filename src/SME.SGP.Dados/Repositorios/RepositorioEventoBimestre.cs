using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoBimestre : RepositorioBase<EventoBimestre>, IRepositorioEventoBimestre

    {
        public RepositorioEventoBimestre(ISgpContext conexao) : base(conexao)
        {
        }
    }
}