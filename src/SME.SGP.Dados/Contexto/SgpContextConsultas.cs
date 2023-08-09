﻿using System.Data;
using Microsoft.Extensions.Configuration;
using SME.SGP.Infra.Interfaces;

namespace SME.SGP.Dados.Contexto
{
    public class SgpContextConsultas : SgpContext, ISgpContextConsultas
    {
        public SgpContextConsultas(IConfiguration configuration, IContextoAplicacao contextoAplicacao)
            : base(configuration, contextoAplicacao, "SGP_PostgresConsultas")
        {
        }

        public SgpContextConsultas(IDbConnection conexao, IContextoAplicacao contextoAplicacao): base(conexao, contextoAplicacao)
        {
        
        }
    }
}
