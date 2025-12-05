using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.SecaoItinerancia
{
    public class Ao_obter_profissionais_envolvidos_atendimento_naapa : AtendimentoNAAPATesteBase
    {
        public Ao_obter_profissionais_envolvidos_atendimento_naapa(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosDreOuUePorPerfisQuery, IEnumerable<FuncionarioUnidadeDto>>), typeof(ObterFuncionariosDrePerfisQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Encaminhamento NAAPA - Obter profissionais envolvidos atendimento por dre")]
        public async Task Ao_obter_profissionais_envolvidos_atendimento_por_dre()
        {
            var filtroNAAPA = new FiltroNAAPADto()
            {
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                Modalidade = Modalidade.Fundamental,
                AnoTurma = "8",
                DreId = 1,
                CodigoUe = "1",
                TurmaId = TURMA_ID_1,
                Situacao = (int)SituacaoNAAPA.Rascunho,
                Prioridade = NORMAL
            };
            await CriarDadosBase(filtroNAAPA);
            await CriarAtribuicoesResponsaveis();
            var useCase = ObterServicoObtencaoProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase();
            var retorno = await useCase.Executar(new Infra.Dtos.FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA(DRE_CODIGO_1));
            retorno.Count().ShouldBe(4);
            retorno.Any(r => r.Login.Equals("00001") && r.Perfil.Equals(Perfis.PERFIL_PSICOLOGO_ESCOLAR)).ShouldBeTrue();
            retorno.Any(r => r.Login.Equals("00002") && r.Perfil.Equals(Perfis.PERFIL_PSICOPEDAGOGO)).ShouldBeTrue();
            retorno.Any(r => r.Login.Equals("00003") && r.Perfil.Equals(Perfis.PERFIL_ASSISTENTE_SOCIAL)).ShouldBeTrue();
            retorno.Any(r => r.Login.Equals("00004") && r.Perfil.Equals(Perfis.PERFIL_COORDENADOR_NAAPA)).ShouldBeTrue();
        }

        private async Task CriarAtribuicoesResponsaveis()
        {
            await InserirNaBase(new Usuario
            {
                Login = "00001",
                CodigoRf = "00001",
                Nome = "Usuário Psicólogo",
                PerfilAtual = Guid.Parse(PerfilUsuario.PSICOLOGO_ESCOLAR.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SupervisorEscolaDre
            {
                DreId = DRE_CODIGO_1,
                Tipo = (int)TipoResponsavelAtribuicao.PsicologoEscolar,
                SupervisorId = "00001",
                EscolaId = UE_CODIGO_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Usuario
            {
                Login = "00002",
                CodigoRf = "00002",
                Nome = "Usuário Psicopedagogo",
                PerfilAtual = Guid.Parse(PerfilUsuario.PSICOPEDAGOGO.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SupervisorEscolaDre
            {
                DreId = DRE_CODIGO_1,
                Tipo = (int)TipoResponsavelAtribuicao.Psicopedagogo,
                SupervisorId = "00002",
                EscolaId = UE_CODIGO_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new Usuario
            {
                Login = "00003",
                CodigoRf = "00003",
                Nome = "Usuário Assistente Social",
                PerfilAtual = Guid.Parse(PerfilUsuario.ASSISTENTE_SOCIAL.ObterNome()),
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

            await InserirNaBase(new SupervisorEscolaDre
            {
                DreId = DRE_CODIGO_1,
                Tipo = (int)TipoResponsavelAtribuicao.AssistenteSocial,
                SupervisorId = "00003",
                EscolaId = UE_CODIGO_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF
            });

        }
    }
}