using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
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
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact]
        public async Task Criar_justificativa_somente_com_descricao()
        {
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                Anotacao = ANOTACAO_FREQUENCIA_ALUNO,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact]
        public async Task Criar_justificativa_com_motivo_e_descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = ANOTACAO_FREQUENCIA_ALUNO,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact]
        public async Task Nao_Deve_Criar_justificativa_somente_com_motivo_Sem_Aula()
        {
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Aula_nao_encontrada_anotacao);
        }

        [Fact]
        public async Task Nao_Deve_Criar_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Data()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_139.ToString(), DateTime.Now.AddDays(-10), RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_139,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Nao_pode_fazer_alteracoes_anotacao_nesta_turma_componente_e_data);
        }

        [Fact]
        public async Task Nao_Deve_Criar_justificativa_Crianca_nao_encontrada_anotacao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_03_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_77777,
                EhInfantil = true
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Crianca_nao_encontrada_anotacao);
        }

        [Fact]
        public async Task Nao_Deve_Criar_justificativa_Aluno_nao_encontrado_anotacao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_03_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_77777,
                EhInfantil = false
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Aluno_nao_encontrado_anotacao);
        }

        [Fact]
        public async Task Nao_Deve_Criar_justificativa_Motivo_ausencia_nao_encontrado()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ENCHENTE_6,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd));

            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Motivo_ausencia_nao_encontrado);
        }


        [Fact]
        public async Task Excluir_justificativa()
        {
           var deletar =  await ExcluirAnotacaoFrequenciaAlunoUseCase(await Criar_Justificativa_Para_Exclusao_Alteracao());
            deletar.ShouldBeTrue();
        }

        [Fact]
        public async Task Nao_Deve_Excluir_justificativa_Anotacao_Nao_Localizada_Com_Id_Informado()
        {
            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await ExcluirAnotacaoFrequenciaAlunoUseCase(8));
            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Anotacao_nao_localizada_com_id_informado);
        }


        [Fact]
        public async Task Nao_Deve_Excluir_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Data()
        {
            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await ExcluirAnotacaoFrequenciaAlunoUseCase(await Criar_Justificativa_Para_Exclusao_Possui_Anotacao()));
            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Nao_pode_fazer_alteracoes_anotacao_nesta_turma_componente_e_data);
        }

        public async Task Alterar_justificativa_com_motivo_e_descricao()
        {

        }

        public async Task Alterar_justificativa_somente_com_descricao_sem_motivo()
        {

        }

        public async Task Alterar_justificativa_somente_com_motivo_sem_descricao() 
        {

        }

        public async Task Altear_justificativa_somente_motivo_com_descricao()
        {

        }

        public async Task Alterar_justificativa_somente_descricao_com_motivo()
        {

        }

        public async Task Nao_Deve_Alterar_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Dato()
        {

        }

        public async Task Nao_Deve_Alterar_justificativa_Anotacao_Nao_Localizada_Com_Id_Informado()
        {
            var excecao = await Assert.ThrowsAsync<NegocioException>(async () => await ExcluirAnotacaoFrequenciaAlunoUseCase(8));
            excecao.Message.ShouldBe(MensagensNegocioFrequencia.Anotacao_nao_localizada_com_id_informado);
        }

        private async Task<long> Criar_Justificativa_Para_Exclusao_Alteracao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = ANOTACAO_FREQUENCIA_ALUNO,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }
        private async Task<long> Criar_Justificativa_Para_Exclusao_Possui_Anotacao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBase(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_139.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = ANOTACAO_FREQUENCIA_ALUNO,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_139,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await SalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            return retorno.Id;
        }

    }
}
