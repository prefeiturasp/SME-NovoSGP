using Dapper;
using Dommel;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioObjetivoAprendizagemConsulta : IRepositorioObjetivoAprendizagemConsulta
    {
        private readonly ISgpContextConsultas contextoConsulta;
        public RepositorioObjetivoAprendizagemConsulta(ISgpContextConsultas contextoConsulta)
        {
            this.contextoConsulta = contextoConsulta ?? throw new ArgumentNullException(nameof(contextoConsulta)); 
        }

        public async Task<IEnumerable<ObjetivoAprendizagemDto>> ObterPorAnoEComponenteCurricularJuremaIds(AnoTurma? ano, long[] juremaIds)
        => await contextoConsulta.Conexao.QueryAsync<ObjetivoAprendizagemDto>($@"select id, descricao, codigo, 
                    ano_turma as ano, componente_curricular_id as idComponenteCurricular, componente_curricular_id as ComponenteCurricularEolId 
                    from objetivo_aprendizagem 
                    where  componente_curricular_id = ANY(@componentes) and excluido = false
                    {(ano.HasValue ? " and ano_turma = @ano " : "")} ",
                    new { ano = ano?.Name(), componentes = juremaIds });
    }
}