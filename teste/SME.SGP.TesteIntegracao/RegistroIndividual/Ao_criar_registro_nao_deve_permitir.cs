using System;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_criar_registro_nao_deve_permitir : RegistroIndividualTesteBase
    {
        public Ao_criar_registro_nao_deve_permitir(CollectionFixture collectionFixture) : base(collectionFixture)
        {}
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em data futura (não deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_em_data_futura_nao_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            var filtro = ObterFiltroRegistroIndividualDto();
            filtro.CriarPeriodoReabertura = false;
            
            await CriarDadosBasicos(filtro);

            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                Data = DateTimeExtension.HorarioBrasilia().Date.AddDays(1),
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            await Should.ThrowAsync<NegocioException>(() => inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto));
        }

        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em data anterior em bimestre encerrado sem reabertura (não deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_em_data_anterior_bimestre_encerrado_sem_reabertura_nao_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            var filtro = ObterFiltroRegistroIndividualDto();
            filtro.CriarPeriodoReabertura = false;
            
            await CriarDadosBasicos(filtro);

            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                Data = DateTimeExtension.HorarioBrasilia().AddDays(-210).Date,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            await Should.ThrowAsync<NegocioException>(() => inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto));
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em data anterior em bimestre válido (deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_em_data_anterior_bimestre_aberto_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            var filtro = ObterFiltroRegistroIndividualDto();
            filtro.CriarPeriodoReabertura = false;
            
            await CriarDadosBasicos(filtro);

            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                Data = DateTimeExtension.HorarioBrasilia().Date,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            var retorno = await inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);
            
            var registrosIndividuais = ObterTodos<Dominio.RegistroIndividual>();
            registrosIndividuais.Any().ShouldBeTrue();
            registrosIndividuais.FirstOrDefault().Id.ShouldBe(1);
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual para turma de ano anterior (não deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_em_ano_anterior_nao_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            var filtro = ObterFiltroRegistroIndividualDto();
            filtro.CriarPeriodoReabertura = false;
            filtro.EhAnoAnterior = true;
            
            await CriarDadosBasicos(filtro);
            
            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_1),
                Data = DateTimeExtension.HorarioBrasilia().Date,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            await Should.ThrowAsync<NegocioException>(() => inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto));
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual para criança inativa (não deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_para_crianca_inativa_nao_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            var filtro = ObterFiltroRegistroIndividualDto();
            filtro.EhAnoAnterior = true;
            
            await CriarDadosBasicos(filtro);
            
            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_6),
                Data = DateTimeExtension.HorarioBrasilia().Date,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            await Should.ThrowAsync<NegocioException>(() => inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto));
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual para criança nova antes da data de ativação na turma (não deve permitir)")]
        public async Task Ao_cadastrar_registro_individual_para_crianca_nova_antes_data_ativacao_nao_deve_permitir()
        {
            var inserirRegistroIndividualUseCase = ObterServicoInserirRegistroIndividualUseCase();

            await CriarDadosBasicos(ObterFiltroRegistroIndividualDto(true));
            var dataReferencia = DATA_DESISTENCIA_ALUNO_5_REGISTRO_INDIVIDUAL.AddDays(5);
            await CriarPeriodoEscolar(dataReferencia, dataReferencia.AddDays(10), BIMESTRE_1, TIPO_CALENDARIO_1);

            var planoAeePersistenciaDto = new InserirRegistroIndividualDto()
            {
                TurmaId = TURMA_ID_1,
                AlunoCodigo = long.Parse(ALUNO_CODIGO_5),
                Data = dataReferencia,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                Registro = DESCRICAO_REGISTRO_INDIVIDUAL
            };
            
            var retorno = await inserirRegistroIndividualUseCase.Executar(planoAeePersistenciaDto);
            retorno.ShouldBeNull();
        }
        private FiltroRegistroIndividualDto ObterFiltroRegistroIndividualDto(bool naoCriarPeriodosEscolares = false)
        {
            return new FiltroRegistroIndividualDto()
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                NaoCriarPeriodosEscolares = naoCriarPeriodosEscolares
            };
        }

    }
}