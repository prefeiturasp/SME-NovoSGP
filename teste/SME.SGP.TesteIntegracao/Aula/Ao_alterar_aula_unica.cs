using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.AulaUnica
{
    public class Ao_alterar_aula_unica : AulaTeste
    {
        public Ao_alterar_aula_unica(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
        }


        [Fact(DisplayName = "Aula - Não deve permitir criar aula para um dia que já existe para o componente")]
        public async Task Ja_existe_aula_criada_no_dia_para_o_componente()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 2;

            await CriarPeriodoEscolarEAbertura();

            await useCase.Executar(dto).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Aula - Não deve permitir alterar aula fora do período")]
        public async Task Nao_e_possivel_alterar_aula_fora_do_periodo()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarPeriodoEscolarEncerrado();

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            var useCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var dto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);

            dto.Id = 1;

            await useCase.Executar(dto).ShouldThrowAsync<NegocioException>();
        }

        [Fact(DisplayName = "Aula - Deve permitir diminuir a quantidade de aula cadastrada para professor fundamental")]
        public async Task Diminuir_quantidade_aula_para_professor_fundamental()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();
            
            var alterarAulaUseCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var persistirAulaDto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
            persistirAulaDto.Id = 1;
            persistirAulaDto.Quantidade = 1; //Antes 3, agora 1

            var retorno = await alterarAulaUseCase.Executar(persistirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Quantidade.ShouldBe(1);
            
            //Chama a fila rabbit para atualizar a frequencia
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new AulaAlterarFrequenciaRequestDto(AULA_ID,3)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
            
            var alterarAulaFrequencia = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();
            await alterarAulaFrequencia.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Count().Equals(1).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeFalse();
            compensacoesCompensacaoAusenciaAlunos.FirstOrDefault().QuantidadeFaltasCompensadas.Equals(1).ShouldBeTrue();

            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Count().Equals(3).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> a.Excluido).Equals(2).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> !a.Excluido).Equals(1).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Aula - Deve permitir aumentar a quantidade de aula cadastrada para professor fundamental")]
        public async Task Aumentar_quantidade_de_aula()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2, false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();
            
            var alterarAulaUseCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var persistirAulaDto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
            persistirAulaDto.Id = 1;
            persistirAulaDto.Quantidade = 10; //Antes 3, agora 10

            var retorno = await alterarAulaUseCase.Executar(persistirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Quantidade.ShouldBe(10);
            
            //Chama a fila rabbit para atualizar a frequencia
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new AulaAlterarFrequenciaRequestDto(AULA_ID,3)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
            
            var alterarAulaFrequencia = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();
            await alterarAulaFrequencia.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Count().Equals(1).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeFalse();
            compensacoesCompensacaoAusenciaAlunos.FirstOrDefault().QuantidadeFaltasCompensadas.Equals(3).ShouldBeTrue();
            
            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Count().Equals(3).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> a.Excluido).Equals(0).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> !a.Excluido).Equals(3).ShouldBeTrue();
        }

        [Fact(DisplayName = "Aula - Deve permitir diminuir a quantidade de aula cadastrada para regência")]
        public async Task Diminuir_quantidade_aula_para_regente()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2,false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();
            
            var alterarAulaUseCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var persistirAulaDto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
            persistirAulaDto.Id = 1;
            persistirAulaDto.Quantidade = 1;
            persistirAulaDto.EhRegencia = true;

            var retorno = await alterarAulaUseCase.Executar(persistirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Quantidade.ShouldBe(1);
            
            //Chama a fila rabbit para atualizar a frequencia
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new AulaAlterarFrequenciaRequestDto(AULA_ID,3)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
            
            var alterarAulaFrequencia = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();
            await alterarAulaFrequencia.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Count().Equals(1).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeFalse();
            compensacoesCompensacaoAusenciaAlunos.FirstOrDefault().QuantidadeFaltasCompensadas.Equals(1).ShouldBeTrue();
            
            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Count().Equals(3).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> a.Excluido).Equals(2).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> !a.Excluido).Equals(1).ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Aula - Deve permitir aumentar a quantidade de aula cadastrada para regência")]
        public async Task Aumentar_quantidade_aula_para_regente()
        {
            await CriarDadosBasicosAula(ObterPerfilProfessor(), Modalidade.Fundamental, ModalidadeTipoCalendario.FundamentalMedio, DATA_02_05, DATA_08_07, BIMESTRE_2,false);

            await CriarAula(COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(), DATA_02_05, RecorrenciaAula.AulaUnica, USUARIO_PROFESSOR_CODIGO_RF_1111111);

            await CriarPeriodoEscolarEAbertura();
            
            await CriarFrequencia(); 
                
            await CriarCompensacaoAusencia();
            
            var alterarAulaUseCase = ServiceProvider.GetService<IAlterarAulaUseCase>();

            var persistirAulaDto = ObterAula(TipoAula.Normal, RecorrenciaAula.AulaUnica, COMPONENTE_CURRICULAR_PORTUGUES_ID_138, DATA_02_05);
            persistirAulaDto.Id = 1;
            persistirAulaDto.Quantidade = 10;
            persistirAulaDto.EhRegencia = true;

            var retorno = await alterarAulaUseCase.Executar(persistirAulaDto);
            retorno.ShouldNotBeNull();

            var aulas = ObterTodos<Dominio.Aula>();
            aulas.ShouldNotBeEmpty();
            aulas.FirstOrDefault().Quantidade.ShouldBe(10);
            
            //Chama a fila rabbit para atualizar a frequencia
            var mensagem = new MensagemRabbit(
                JsonConvert.SerializeObject(new AulaAlterarFrequenciaRequestDto(AULA_ID,3)),
                Guid.NewGuid(),
                USUARIO_PROFESSOR_LOGIN_2222222,
                USUARIO_PROFESSOR_LOGIN_2222222,
                Guid.Parse(PerfilUsuario.PROFESSOR.Name()),
                false,
                TesteBaseComuns.USUARIO_ADMIN_RF);
            
            var alterarAulaFrequencia = ServiceProvider.GetService<IAlterarAulaFrequenciaTratarUseCase>();
            await alterarAulaFrequencia.Executar(mensagem);
            
            var compensacoesCompensacaoAusenciaAlunos = ObterTodos<Dominio.CompensacaoAusenciaAluno>();
            compensacoesCompensacaoAusenciaAlunos.Count().Equals(1).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> !a.Excluido).ShouldBeTrue();
            compensacoesCompensacaoAusenciaAlunos.Any(a=> a.Excluido).ShouldBeFalse();
            compensacoesCompensacaoAusenciaAlunos.FirstOrDefault().QuantidadeFaltasCompensadas.Equals(3).ShouldBeTrue();
            
            var compensacaoAusenciaAlunoAula = ObterTodos<Dominio.CompensacaoAusenciaAlunoAula>();
            compensacaoAusenciaAlunoAula.Count().Equals(3).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> a.Excluido).Equals(0).ShouldBeTrue();
            compensacaoAusenciaAlunoAula.Count(a=> !a.Excluido).Equals(3).ShouldBeTrue();
        }

        private async Task CriarPeriodoEscolarEAbertura()
        {
            await CriarPeriodoEscolar(DATA_03_01_INICIO_BIMESTRE_1, DATA_01_05_FIM_BIMESTRE_1, BIMESTRE_1);

            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);

            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);

            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);

            await CriarPeriodoReabertura(TIPO_CALENDARIO_1);
        }
        
         private async Task CriarFrequencia()
        {
            await InserirNaBase(new RegistroFrequencia
            {
                AulaId = AULA_ID,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_1, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_2, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new RegistroFrequenciaAluno()
            {
                Valor = (int)TipoFrequencia.F, 
                CodigoAluno = ALUNO_CODIGO_1, 
                NumeroAula = NUMERO_AULA_3, 
                RegistroFrequenciaId = 1, 
                AulaId = AULA_ID_1, 
                CriadoEm = DateTimeExtension.HorarioBrasilia().Date, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }

        private async Task CriarCompensacaoAusencia()
        {
            await InserirNaBase(new CompensacaoAusencia
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                DisciplinaId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138.ToString(),
                Bimestre = BIMESTRE_2,
                TurmaId = TURMA_ID_1,
                Nome = "Atividade de compensação",
                Descricao = "Breve descrição da atividade de compensação",
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAluno
            {
                CodigoAluno = CODIGO_ALUNO_1,
                CompensacaoAusenciaId = 1,
                QuantidadeFaltasCompensadas = NUMERO_AULA_3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_1,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 1,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_2,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 2,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new CompensacaoAusenciaAlunoAula()
            {
                DataAula = DATA_02_05,
                NumeroAula = NUMERO_AULA_3,
                CompensacaoAusenciaAlunoId = 1,
                RegistroFrequenciaAlunoId = 3,
                CriadoEm = DateTimeExtension.HorarioBrasilia(), CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
        }
    }
}
