using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Ocorrencia.Base;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using OcorrenciaObj = SME.SGP.Dominio.Ocorrencia;

namespace SME.SGP.TesteIntegracao.Ocorrencia
{
    public class Ao_Incluir_ocorrencia_com_turma : OcorrenciaTesteBase
    {
        public Ao_Incluir_ocorrencia_com_turma(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência para turma sem aluno")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_para_turma_sem_aluno(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(modalidade);
            var descricaoTitulo = "Ocorrência sem aluno";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
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
            ocorrencia.TurmaId.ShouldBe(TURMA_ID_1);

            var ocorrenciasAlunos = ObterTodos<OcorrenciaAluno>();
            ocorrenciasAlunos.ShouldNotBeNull();
            ocorrenciasAlunos.Any().ShouldBeFalse();
        }

        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência para turma com aluno")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_para_turma_com_aluno(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(modalidade);
            var descricaoTitulo = "Ocorrência com aluno";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                Titulo = descricaoTitulo,
                CodigosAlunos = new List<long>() { ALUNO_1, ALUNO_2, ALUNO_3 }
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);
            ocorrencia.TurmaId.ShouldBe(TURMA_ID_1);

            var ocorrenciasAlunos = ObterTodos<OcorrenciaAluno>();
            ocorrenciasAlunos.ShouldNotBeNull();
            ocorrenciasAlunos.Count().ShouldBe(3);
            ocorrenciasAlunos.Exists(ocorrencia => ocorrencia.CodigoAluno == ALUNO_1).ShouldBeTrue();
        }


        [Theory(DisplayName = "Ocorrência - Deve inserir ocorrência para turma com servidor")]
        [InlineData(PROFESSOR_INFANTIL, Modalidade.EducacaoInfantil)]
        [InlineData(PROFESSOR, Modalidade.Fundamental)]
        public async Task Deve_inserir_ocorrencia_para_turma_com_servidor(string perfil, Modalidade modalidade)
        {
            await CriarDadosBasicos(modalidade);
            var descricaoTitulo = "Ocorrência com servidor";
            var useCase = InserirOcorrenciaUseCase();
            var dtoInserir = new InserirOcorrenciaDto()
            {
                AnoLetivo = DateTimeExtension.HorarioBrasilia().Year,
                UeId = UE_ID_1,
                TurmaId = TURMA_ID_1,
                Descricao = "Ocorrência de incidente",
                DataOcorrencia = DateTimeExtension.HorarioBrasilia(),
                Modalidade = (int)modalidade,
                HoraOcorrencia = DateTimeExtension.HorarioBrasilia().ToString("HH:mm"),
                OcorrenciaTipoId = ID_TIPO_INCIDENTE,
                Titulo = descricaoTitulo,
                CodigosServidores = new List<string>() { RF_3333333, RF_4444444 }
            };

            var auditoria = await useCase.Executar(dtoInserir);
            auditoria.ShouldNotBeNull();

            var ocorrencias = ObterTodos<OcorrenciaObj>();
            ocorrencias.ShouldNotBeNull();
            var ocorrencia = ocorrencias.FirstOrDefault();
            ocorrencia.Titulo.ShouldBe(descricaoTitulo);
            ocorrencia.TurmaId.ShouldBe(TURMA_ID_1);

            var ocorrenciasServidores = ObterTodos<OcorrenciaServidor>();
            ocorrenciasServidores.ShouldNotBeNull();
            ocorrenciasServidores.Count().ShouldBe(2);
            ocorrenciasServidores.Exists(ocorrencia => ocorrencia.CodigoServidor == RF_3333333).ShouldBeTrue();
        }
    }
}
