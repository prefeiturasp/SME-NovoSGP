using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPapPainelEducacionalConsolidacao : IRepositorioPapPainelEducacionalConsolidacao
    {
        private readonly ISgpContext database;

        public RepositorioPapPainelEducacionalConsolidacao(ISgpContext database)
        {
            this.database = database ?? throw new System.ArgumentNullException(nameof(database));
        }

        public async Task LimparConsolidacao()
        {
            var sql = "DELETE FROM consolidacao_informacoes_pap";
            await database.ExecuteAsync(sql);
        }

        public async Task<bool> Inserir(IEnumerable<ConsolidacaoInformacoesPap> indicadoresPap)
        {
            if (!indicadoresPap?.Any() == true)
                return false;

            var sql = @"INSERT INTO consolidacao_informacoes_pap 
                        (tipo_pap, dre_codigo, ue_codigo, dre_nome, ue_nome, quantidade_turmas, quantidade_estudantes, 
                         quantidade_estudantes_com_frequencia_inferior_limite, 
                         quantidade_estudantes_dificuldade_top_1, quantidade_estudantes_dificuldade_top_2, 
                         outras_dificuldades_aprendizagem, nome_dificuldade_top_1, nome_dificuldade_top_2)
                        VALUES (@TipoPapNome, @DreCodigo, @UeCodigo, @DreNome, @UeNome, @QuantidadeTurmas, @QuantidadeEstudantes, 
                                @QuantidadeEstudantesComFrequenciaInferiorLimite, 
                                @QuantidadeEstudantesDificuldadeTop1, @QuantidadeEstudantesDificuldadeTop2, 
                                @OutrasDificuldadesAprendizagem, @NomeDificuldadeTop1, @NomeDificuldadeTop2)";

            var parametros = indicadoresPap.Select(indicador => new
            {
                TipoPapNome = indicador.TipoPap.ObterNome(),
                indicador.DreCodigo,
                indicador.UeCodigo,
                indicador.DreNome,
                indicador.UeNome,
                indicador.QuantidadeTurmas,
                indicador.QuantidadeEstudantes,
                indicador.QuantidadeEstudantesComFrequenciaInferiorLimite,
                indicador.QuantidadeEstudantesDificuldadeTop1,
                indicador.QuantidadeEstudantesDificuldadeTop2,
                indicador.OutrasDificuldadesAprendizagem,
                indicador.NomeDificuldadeTop1,
                indicador.NomeDificuldadeTop2
            });

            var resultado = await database.ExecuteAsync(sql, parametros);
            return resultado > 0;
        }
    }
}