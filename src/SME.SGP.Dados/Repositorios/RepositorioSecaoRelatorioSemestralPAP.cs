﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoRelatorioSemestralPAP : IRepositorioSecaoRelatorioSemestralPAP
    {
        private readonly ISgpContext database;

        public RepositorioSecaoRelatorioSemestralPAP(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<IEnumerable<SecaoRelatorioSemestralPAP>> ObterSecoes()
        {
            var query = @"select id, nome, descricao, obrigatorio 
                          from public.secao_relatorio_semestral_pap";

            return await database.Conexao.QueryAsync<SecaoRelatorioSemestralPAP>(query, new {  });
        }
    }
}
