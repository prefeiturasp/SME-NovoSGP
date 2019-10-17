using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioTipoEvento : RepositorioBase<EventoTipo>, IRepositorioTipoEvento
    {
        public RepositorioTipoEvento(ISgpContext conexao) : base(conexao)
        {

        }
    }
}
