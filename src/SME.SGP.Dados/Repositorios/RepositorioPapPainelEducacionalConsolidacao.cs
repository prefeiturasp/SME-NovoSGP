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
                        (tipo_pap, quantidade_turmas, quantidade_estudantes, 
                         quantidade_estudantes_com_menos_75_por_cento_frequencia, 
                         dificuldade_aprendizagem_1, dificuldade_aprendizagem_2, 
                         outras_dificuldades_aprendizagem)
                        VALUES (@TipoPapNome, @QuantidadeTurmas, @QuantidadeEstudantes, 
                                @QuantidadeEstudantesComMenosDe75PorcentoFrequencia, 
                                @DificuldadeAprendizagem1, @DificuldadeAprendizagem2, 
                                @OutrasDificuldadesAprendizagem)";

            var parametros = indicadoresPap.Select(indicador => new
            {
                TipoPapNome = indicador.TipoPap.ObterNome(),
                indicador.QuantidadeTurmas,
                indicador.QuantidadeEstudantes,
                indicador.QuantidadeEstudantesComMenosDe75PorcentoFrequencia,
                indicador.DificuldadeAprendizagem1,
                indicador.DificuldadeAprendizagem2,
                indicador.OutrasDificuldadesAprendizagem
            });

            var resultado = await database.ExecuteAsync(sql, parametros);
            return resultado > 0;
        }
    }
}