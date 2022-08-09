using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace SME.SGP.TesteIntegracao.ConselhoDeClasseLancamento
{
    public class Ao_inserir_nota_pos_conselho_bimestre : ConselhoDeClasseLancamentoBase
    {
        private const string JUSTIFICATIVA = "Nota pós conselho";
        public Ao_inserir_nota_pos_conselho_bimestre(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Ao_lancar_nota_pos_conselhor_bimestre_numerica_fundamental() 
        {
            await CrieDados(ObterPerfilProfessor(), TipoNota.Nota, ANO_7, Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio);

            var comando = ServiceProvider.GetService<IComandosConselhoClasseNota>();

            var dto = new ConselhoClasseNotaDto()
            {
                CodigoComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                Nota = 7,
                Justificativa = JUSTIFICATIVA
            };

            var dtoRetorno = await comando.SalvarAsync(dto, ALUNO_CODIGO_1, CONSELHO_CLASSE_ID, FECHAMENTO_TURMA_ID, TURMA_CODIGO_1, BIMESTRE_2);
            dtoRetorno.ShouldBeNull();
            var listaConselhoClasseNota = ObterTodos<ConselhoClasseNota>();
            listaConselhoClasseNota.ShouldBeNull();
           // var lista listaConselhoClasseNota.FirstOrDefault(nota => nota.ConselhoClasseAlunoId = 1);

        }

        private async Task CrieDados(
                        string perfil, 
                        TipoNota tipo, 
                        string anoTurma, 
                        Modalidade modalidade,
                        ModalidadeTipoCalendario modalidadeTipoCalendario)
        {
            var filtroNota = new FiltroNotasDto()
            {
                Perfil = perfil,
                Modalidade = modalidade ,
                TipoCalendario = modalidadeTipoCalendario,
                Bimestre = BIMESTRE_2,
                ComponenteCurricular = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                TipoNota = tipo,
                AnoTurma = anoTurma
            };

            await CriarDadosBase(filtroNota);
            await CriarAula(filtroNota.ComponenteCurricular, DATA_02_05_INICIO_BIMESTRE_2, RecorrenciaAula.AulaUnica, NUMERO_AULA_1);
            await CrieTipoAtividade();
            await CriarAtividadeAvaliativa(DATA_02_05_INICIO_BIMESTRE_2, filtroNota.ComponenteCurricular, USUARIO_PROFESSOR_LOGIN_1111111, true, ATIVIDADE_AVALIATIVA_1);
        }
    }
}
