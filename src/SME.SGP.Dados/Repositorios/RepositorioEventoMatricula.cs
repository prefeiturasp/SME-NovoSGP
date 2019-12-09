using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioEventoMatricula: RepositorioBase<EventoMatricula>, IRepositorioEventoMatricula
    {
        public RepositorioEventoMatricula(ISgpContext conexao) : base(conexao)
        {
        }
    }
}
