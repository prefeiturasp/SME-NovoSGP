﻿using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeInfantil : RepositorioBase<AtividadeInfantil>, IRepositorioAtividadeInfantil
    {
        public RepositorioAtividadeInfantil(ISgpContext context, IServicoAuditoria servicoAuditoria) : base(context, servicoAuditoria)
        { }

        public async Task<IEnumerable<AtividadeInfantilDto>> ObterPorAulaId(long aulaId)
        {
            var query = @"select id
                            , titulo
                            , mensagem
                            , email 
                            , criado_em as DataPublicacao
                         from atividade_infantil 
                        where aula_id = @aulaId";

            return await database.Conexao.QueryAsync<AtividadeInfantilDto>(query, new { aulaId });
        }

        public async Task<AtividadeInfantil> ObterPorAtividadeClassroomId(long atividadeClassroomId)
        {
            var query = @"select * from atividade_infantil where atividade_classroom_id = @atividadeClassroomId";
            return await database.Conexao.QueryFirstOrDefaultAsync<AtividadeInfantil>(query, new { atividadeClassroomId });
        }
    }
}
