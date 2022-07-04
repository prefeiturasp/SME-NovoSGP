using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_realizar_lancamento_de_justificativa : FrequenciaBase
    {
        public Ao_realizar_lancamento_de_justificativa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact]
        public async Task Criar_justificativa_somente_com_motivo()
        {
            await CriarMotivosAusencias(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            var useCase = ServiceProvider.GetService<ISalvarAnotacaoFrequenciaAlunoUseCase>();
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var salvar = useCase.Executar(parametrosFrontEnd);
            //var excecao = await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(dto));

            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Aula_nao_encontrada_anotacao);
            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Nao_pode_fazer_alteracoes_anotacao_nesta_turma_componente_e_data);
            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Crianca_nao_encontrada_anotacao);
            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Aluno_nao_encontrado_anotacao);
            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Aula_nao_encontrada_anotacao);
            //excecao.Message.ShouldNotBe(MensagensNegocioFrequencia.Motivo_ausencia_nao_encontrado);

            salvar.ShouldBeNull(salvar.Exception.ToString());
            salvar.IsCompletedSuccessfully.ShouldBeTrue();
            //salvar.ShouldBe(salvar.Id > 0);
        }
        [Fact]
        public async Task Criar_justificativa_somente_com_descricao()
        {

        }

        [Fact]
        public async Task Criar_justificativa_com_motivo_e_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_com_motivo_e_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_com_descricao_sem_motivo()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_com_motivo_sem_descricao() 
        {

        }

        [Fact]
        public async Task Altear_justificativa_somente_motivo_com_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_descricao_com_motivo()
        {

        }

        [Fact]
        public async Task Excluir_justificativa()
        {

        }
    }
}
