using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Ocorrencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using OcorrenciaObj = SME.SGP.Dominio.Ocorrencia;

namespace SME.SGP.TesteIntegracao.Ocorrencia
{
    public class Ao_Incluir_ocorrencia_sem_turma : OcorrenciaTesteBase
    {
        public Ao_Incluir_ocorrencia_sem_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Ocorrência - Não deve inserir ocorrência para data futura")]
        public async Task Nao_deve_inserir_ocorrencia_para_data_futura()
        {
            await CriarDadosBasicos(ObterPerfilProfessorInfantil());
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia().AddDays(1),
                Modalidade = (int)Modalidade.EducacaoInfantil,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = 1,
                Titulo = "Ocorrência data futura"
            };

            await useCase.Executar(dtoInserir).ShouldThrowAsync<NegocioException>(MensagemNegocioOcorrencia.Data_da_ocorrencia_nao_pode_ser_futura);
        }


        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência com data passada")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_com_data_passada(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(perfil, modalidade);
            var descricaoTitulo = "Ocorrência para data passada";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia().AddDays(-1),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                Titulo = descricaoTitulo
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);

            var ocorrenciasServidores = ObterTodos<OcorrenciaServidor>();
            ocorrenciasServidores.ShouldNotBeNull();
            ocorrenciasServidores.Any().ShouldBeFalse();
        }

        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência com ue com servidores")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_para_ue_com_servidores(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(perfil, modalidade);
            var descricaoTitulo = "Ocorrência com servidor";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                CodigosServidores = new List<string>() { RF_3333333, RF_4444444 },
                Titulo = descricaoTitulo
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);

            var ocorrenciasServidores = ObterTodos<OcorrenciaServidor>();
            ocorrenciasServidores.ShouldNotBeNull();
            ocorrenciasServidores.Count().ShouldBe(2);
            ocorrenciasServidores.Exists(ocorrencia => ocorrencia.CodigoServidor == RF_3333333).ShouldBeTrue();
        }

        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência para ue com alunos e servidores")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_apenas_para_ue_com_alunos_e_servidores(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(perfil, modalidade);
            var descricaoTitulo = "Ocorrência com alunos e servidores";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                CodigosServidores = new List<string>() { RF_3333333, RF_4444444 },
                CodigosAlunos = new List<long>() { ALUNO_1, ALUNO_2, ALUNO_3 },
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                Titulo = descricaoTitulo
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);

            var ocorrenciasServidores = ObterTodos<OcorrenciaServidor>();
            ocorrenciasServidores.ShouldNotBeNull();
            ocorrenciasServidores.Count().ShouldBe(2);
            ocorrenciasServidores.Exists(ocorrencia => ocorrencia.CodigoServidor == RF_3333333).ShouldBeTrue();

            var ocorrenciasAlunos = ObterTodos<OcorrenciaAluno>();
            ocorrenciasAlunos.ShouldNotBeNull();
            ocorrenciasAlunos.Count().ShouldBe(3);
            ocorrenciasAlunos.Exists(ocorrencia => ocorrencia.CodigoAluno == ALUNO_1).ShouldBeTrue();
        }

        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência apenas para ue")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_apenas_para_ue(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(perfil, modalidade);
            var descricaoTitulo = "Ocorrência apenas para ue";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                Titulo = descricaoTitulo
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);

            var ocorrenciasServidores = ObterTodos<OcorrenciaServidor>();
            ocorrenciasServidores.ShouldNotBeNull();
            ocorrenciasServidores.Any().ShouldBeFalse();
        }
    }
}
