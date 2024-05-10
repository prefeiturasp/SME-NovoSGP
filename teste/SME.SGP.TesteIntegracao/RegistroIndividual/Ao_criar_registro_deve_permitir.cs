using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using Xunit;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public class Ao_criar_registro_deve_permitir : RegistroIndividualTesteBase
    {
        public Ao_criar_registro_deve_permitir(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual na data atual")]
        public async Task Cadastrar_registro_individual_na_data_atual()
        {
            var filtro = new FiltroRegistroIndividualDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                BimestreEncerrado = false
            };
            
            var registroParaSalvar = new InserirRegistroIndividualDto
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                AlunoCodigo = NUMERO_LONGO_1,
                Registro = "<pre><span>Registro de teste</span></pre>",
                Data = DateTimeExtension.HorarioBrasilia().Date
            };
            await ExecutarTeste(filtro, registroParaSalvar);
        }

        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em data anterior do bimestre atual")]
        public async Task Cadastrar_registro_individual_em_data_anterior_do_bimestre_atual()
        {
            var filtro = new FiltroRegistroIndividualDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                BimestreEncerrado = false
            };
            
            var registroParaSalvar = new InserirRegistroIndividualDto
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_CODIGO_512,
                AlunoCodigo = NUMERO_LONGO_2,
                Registro = "<pre><span>Registro de teste</span></pre>",
                Data = DateTimeExtension.HorarioBrasilia().Date
            };
            await ExecutarTeste(filtro, registroParaSalvar);
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em data anterior em bimestre encerrado com reabertura")]
        public async Task Cadastrar_registro_individual_em_data_anterior_em_bimestre_encerrado_com_reabertura()
        {
            var filtro = new FiltroRegistroIndividualDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                BimestreEncerrado = true
            };
            
            var registroParaSalvar = new InserirRegistroIndividualDto
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                AlunoCodigo = NUMERO_LONGO_2,
                Registro = "<pre><span>Registro de teste</span></pre>",
                Data = DateTimeExtension.HorarioBrasilia().Date
            };
            await ExecutarTeste(filtro, registroParaSalvar);
        }
        
        [Fact(DisplayName = "Registro Individual - Cadastrar registro individual em dia não letivo (domingo, por exemplo)")]
        public async Task Cadastrar_registro_individual_em_dia_nao_letivo()
        {
            var filtro = new FiltroRegistroIndividualDto
            {
                Perfil = ObterPerfilProfessor(),
                Modalidade = Modalidade.EducacaoInfantil,
                TipoCalendario = ModalidadeTipoCalendario.Infantil,
                BimestreEncerrado = true,
                CriarPeriodoReabertura = true
            };
            
            var dataReferencia = DateTimeExtension.HorarioBrasilia().AddDays(-20);
            var dataDeDomingo = dataReferencia.AddDays(7 - (int) dataReferencia.DayOfWeek);   
            
            var registroParaSalvar = new InserirRegistroIndividualDto
            {
                TurmaId = TURMA_ID_1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_ARTES_ID_139,
                AlunoCodigo = NUMERO_LONGO_2,
                Registro = "<pre><span>Registro de teste</span></pre>",
                Data = dataDeDomingo.Date
            };
            await ExecutarTeste(filtro, registroParaSalvar);
        }

        private async Task ExecutarTeste(FiltroRegistroIndividualDto filtroDadosBasicos,InserirRegistroIndividualDto inserirRegistroIndividualDto)
        {
            var useCase = ObterServicoInserirRegistroIndividualUseCase();
            
            var obterResgistros = ObterTodos<Dominio.RegistroIndividual>();
            obterResgistros.Count.ShouldBeEquivalentTo(0);

            await CriarDadosBasicos(filtroDadosBasicos);
            
            var inserirRegistro = await useCase.Executar(inserirRegistroIndividualDto);
            inserirRegistro.ShouldNotBeNull();
            
            var totalResgistros = ObterTodos<Dominio.RegistroIndividual>();
            totalResgistros.ShouldNotBeNull();
            totalResgistros.Count.ShouldBeEquivalentTo(1);
            
            totalResgistros.FirstOrDefault()?.Id.ShouldBeGreaterThan(0);
            totalResgistros.FirstOrDefault()?.AlunoCodigo.ShouldBeEquivalentTo(inserirRegistroIndividualDto.AlunoCodigo);
            totalResgistros.FirstOrDefault()?.Registro.ShouldBeEquivalentTo(inserirRegistroIndividualDto.Registro);
            totalResgistros.FirstOrDefault()?.DataRegistro.Date.ShouldBeEquivalentTo(inserirRegistroIndividualDto.Data.Date);
        }

    }
}