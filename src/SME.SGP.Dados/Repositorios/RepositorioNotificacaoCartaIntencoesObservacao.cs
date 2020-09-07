using Dapper;
using Dommel;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoCartaIntencoesObservacao : IRepositorioNotificacaoCartaIntencoesObservacao
    {
        private readonly ISgpContext database;

        public RepositorioNotificacaoCartaIntencoesObservacao(ISgpContext database)
        {
            this.database = database;
        }

    }
}
