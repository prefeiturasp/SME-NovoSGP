using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Frequencia.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_realizar_lancamento_de_justificativa : FrequenciaTesteBase
    {
        public Ao_realizar_lancamento_de_justificativa(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverArquivosExcluidosCommand, bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake), ServiceLifetime.Scoped));       
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorTurmaAlunoCodigoFrequenciaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Frequência - Deve Criar justificativa somente com motivo")]
        public async Task Deve_Criar_justificativa_somente_com_motivo()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());

            await CriarDadosBasicos(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08,
                BIMESTRE_2, DATA_07_08, COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), false, quantidadeAula: NUMERO_AULAS_2);

            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve Criar justificativa somente com descricao")]
        public async Task Deve_Criar_justificativa_somente_com_descricao()
        {
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve criar justificativa com motivo e descricao")]
        public async Task Deve_Criar_justificativa_com_motivo_e_descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_08, RecorrenciaAula.AulaUnica, NUMERO_AULAS_2);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            var retorno = await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd);
            retorno.ShouldNotBeNull();
            (retorno.Id > 0).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Não deve criar justificativa somente com motivo sem Aula")]
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

            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Nao deve criar justificativa usuario possui atribuicao na turma na data")]
        public async Task Nao_Deve_Criar_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Data()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_ARTES_ID_139.ToString(), DateTime.Now.AddDays(-10), RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };

            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - nao deve criar justificativa crianca nao encontrada anotacao")]
        public async Task Nao_Deve_Criar_justificativa_Crianca_nao_encontrada_anotacao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_03, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_77777,
                EhInfantil = true
            };
            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Não deve criar justificativa criança não ativa")]
        public async Task Nao_Deve_Criar_justificativa_Crianca_nao_ativa()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_03, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_CRIANCA_NAO_ATIVO_666666,
                EhInfantil = true
            };
            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Não deve criar justificativa criança não ativo")]
        public async Task Nao_Deve_Criar_justificativa_Aluno_nao_ativo()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_03, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_CRIANCA_NAO_ATIVO_666666,
                EhInfantil = false
            };
            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }


        [Fact(DisplayName = "Frequência - Não deve criar justificativa aluno nao encontrado anotacao")]
        public async Task Nao_Deve_Criar_justificativa_Aluno_nao_encontrado_anotacao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_07_03, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_77777,
                EhInfantil = false
            };
            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Não deve criar justificativa motivo ausencia não encontrado")]
        public async Task Nao_Deve_Criar_justificativa_Motivo_ausencia_nao_encontrado()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DO_ALUNO_1.ToString());
            await CriarDadosBaseSemTurma(ObterPerfilProfessor(), ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_07_08, BIMESTRE_2);
            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, NUMERO_AULAS_1);
            await CriarTurma(Modalidade.Fundamental, false);
            var parametrosFrontEnd = new SalvarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ENCHENTE_6,
                AulaId = AULA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                CodigoAluno = CODIGO_ALUNO_99999,
                EhInfantil = false
            };
            await ExecutarSalvarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }


        [Fact(DisplayName = "Frequência - Deve excluir justificativa")]
        public async Task Deve_Excluir_justificativa()
        {
            (await ExecutarExcluirAnotacaoFrequenciaAlunoUseCase(await Criar_Justificativa_Para_Exclusao_Alteracao_Motivo_Descricao())).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Não deve excluir justificativa anotacao nao localizada com id informado")]
        public async Task Nao_Deve_Excluir_justificativa_Anotacao_Nao_Localizada_Com_Id_Informado()
        {

            await ExecutarExcluirAnotacaoFrequenciaAlunoUseCase(8).ShouldThrowAsync<NegocioException>();
        }


        [Fact(DisplayName = "Frequência - Não deve excluir justificativa usuario possui atribuicao na turma na data")]
        public async Task Nao_Deve_Excluir_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Data()
        {
            var criarJustificativa = await Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Anotacao_Possui_Atribuicao_Na_Turma_Na_Data();
            await ExecutarExcluirAnotacaoFrequenciaAlunoUseCase(criarJustificativa).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Deve alterar justificativa com motivo e descricao")]
        public async Task Deve_Alterar_justificativa_com_motivo_e_descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2.ToString());
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2,
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Motivo_Descricao(),
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_2
            };
            (await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve alterar justificativa somente com descricao sem motivo")]
        public async Task Deve_Alterar_justificativa_somente_com_descricao_sem_motivo()
        {
            
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Descricao(),
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_2
            };
            (await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve alterar justificativa somente com motivo sem descricao")]
        public async Task Deve_Alterar_justificativa_somente_com_motivo_sem_descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2.ToString());
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Motivo(),
                MotivoAusenciaId = ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2
            };
            (await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve altear justificativa somente motivo com descricao")]
        public async Task Deve_Altear_justificativa_somente_motivo_com_descricao()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2.ToString());
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Motivo_Descricao(),
            };
            (await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Deve alterar justificativa somente descricao com motivo")]
        public async Task Deve_Alterar_justificativa_somente_descricao_com_motivo()
        {
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_2,
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Motivo_Descricao(),
            };
            (await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd)).ShouldBeTrue();
        }

        [Fact(DisplayName = "Frequência - Não deve alterar justificativa usuario possui atribuicao na turma na dato")]
        public async Task Nao_Deve_Alterar_justificativa_Usuario_Possui_Atribuicao_Na_Turma_Na_Dato()
        {
            await CriarMotivoAusencia(ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2.ToString());
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DE_PESSOA_DA_FAMILIA_2,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_2,
                Id = await Criar_Justificativa_Para_Exclusao_Alteracao_Somente_Com_Anotacao_Possui_Atribuicao_Na_Turma_Na_Data(),
            };
            await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Frequência - Não deve alterar justificativa anotacao não localizada com id informado")]
        public async Task Nao_Deve_Alterar_justificativa_Anotacao_Nao_Localizada_Com_Id_Informado()
        {
            var parametrosFrontEnd = new AlterarAnotacaoFrequenciaAlunoDto
            {
                MotivoAusenciaId = ATESTADO_MEDICO_DO_ALUNO_1,
                Anotacao = DESCRICAO_FREQUENCIA_ALUNO_1,
                Id = 9,
            };
            await ExecutarAlterarAnotacaoFrequenciaAlunoUseCase(parametrosFrontEnd).ShouldThrowAsync<NegocioException>();
        }
    }
}