using Npgsql;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanejamentoAnualObjetivosAprendizagem : RepositorioBase<PlanejamentoAnualObjetivoAprendizagem>, IRepositorioPlanejamentoAnualObjetivosAprendizagem
    {
        public RepositorioPlanejamentoAnualObjetivosAprendizagem(ISgpContext database) : base(database)
        {
        }
        public void SalvarVarios(IEnumerable<PlanejamentoAnualObjetivoAprendizagem> objetivos)
        {
            var sql = @"copy planejamento_anual_objetivos_aprendizagem ( 
                                        planejamento_anual_componente_id,
                                        objetivo_aprendizagem_id,
                                        criado_em,
                                        criado_por,
                                        criado_rf)
                            from
                            stdin (FORMAT binary)";

            using (var writer = ((NpgsqlConnection)database.Conexao).BeginBinaryImport(sql))
            {
                foreach (var objetivo in objetivos)
                {
                    writer.StartRow();
                    writer.Write(objetivo.PlanejamentoAnualComponenteId);
                    writer.Write(objetivo.ObjetivoAprendizagemId);
                    writer.Write(DateTime.Now);
                    writer.Write(database.UsuarioLogadoNomeCompleto);
                    writer.Write(database.UsuarioLogadoRF);
                }
                writer.Complete();
            }
        }
    }
}
