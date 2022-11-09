using System;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_haver_validacao_da_coordenacao : PlanoAEETesteBase
    {
        public Ao_haver_validacao_da_coordenacao(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterParametroSistemaPorTipoEAnoQuery, ParametrosSistema>), typeof(ObterParametroSistemaPorTipoEAnoQueryHanlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Deve criar uma pendência para o cp apos cadastrar o plano")]
        public async Task Deve_criar_pendencia_para_o_cp_apos_cadastro_do_plano()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();
            var planoAEEPersistenciaDto = ObterPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var pendencia = ObterTodos<Pendencia>().FirstOrDefault();
            var pendenciaPerfil = ObterTodos<SME.SGP.Dominio.PendenciaPerfil>().FirstOrDefault();

            pendencia.Id.ShouldBe(pendenciaPerfil.PendenciaId);
            pendenciaPerfil.PerfilCodigo.ShouldBe(PerfilUsuario.CP);
        }

        [Fact(DisplayName = "Plano AEE - Deve criar pendência para o cp após edição do plano")]
        public async Task Deve_criar_pendencia_para_o_cp_apos_edicao_do_plano()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var planoAEEPersistenciaDto = ObterPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            planoAEEPersistenciaDto.Situacao = SituacaoPlanoAEE.ParecerCP;

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var pendencia = ObterTodos<Pendencia>().FirstOrDefault();
            var pendenciaPerfil = ObterTodos<SME.SGP.Dominio.PendenciaPerfil>().FirstOrDefault();

            pendencia.Id.ShouldBe(pendenciaPerfil.PendenciaId);
            pendenciaPerfil.PerfilCodigo.ShouldBe(PerfilUsuario.CP);
        }

        [Fact(DisplayName = "Plano AEE - Deve criar uma pendência para o CEFAI após coordenação salvar o parecer")]
        public async Task Deve_criar_pendencia_CEFAI_apos_coordenacao_salvar_parecer()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);

            var servicoCadastrarParecerCP = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var parecerDto = ObterPlanoAEECadastroParecerDto();

            await servicoCadastrarParecerCP.Executar(retorno.Items.FirstOrDefault().Id,parecerDto);

            var pendencia = ObterTodos<Pendencia>().LastOrDefault();
            var pendenciaPerfil = ObterTodos<SME.SGP.Dominio.PendenciaPerfil>().LastOrDefault();

            pendencia.Id.ShouldBe(pendenciaPerfil.PendenciaId);
            pendenciaPerfil.PerfilCodigo.ShouldBe(PerfilUsuario.CEFAI);

        }
        
        [Fact(DisplayName = "Plano AEE - Não deve atribuir automaticamente paai quando a UE não tiver paai")]
        public async Task Nao_deve_atribuir_automaticamente_paai_quando_a_ue_nao_tiver_paai()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);

            var servicoCadastrarParecerCP = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var parecerDto = ObterPlanoAEECadastroParecerDto();

            await servicoCadastrarParecerCP.Executar(retorno.Items.FirstOrDefault().Id,parecerDto);

            var planoAee = ObterTodos<Dominio.PlanoAEE>();
            planoAee.FirstOrDefault().Situacao = SituacaoPlanoAEE.AtribuicaoPAAI;
            
            var pendencia = ObterTodos<Pendencia>().LastOrDefault();
            var pendenciaPerfil = ObterTodos<PendenciaPerfil>().LastOrDefault();

            pendencia.Id.ShouldBe(pendenciaPerfil.PendenciaId);
            pendenciaPerfil.PerfilCodigo.ShouldBe(PerfilUsuario.CEFAI);
        }
        
        [Fact(DisplayName = "Plano AEE - Não deve atribuir automaticamente paai quando a UE tiver mais de um paai")]
        public async Task Nao_deve_atribuir_automaticamente_paai_quando_a_ue_tiver_mais_de_um_paai()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new SupervisorEscolaDre()
            {
                SupervisorId = "1", EscolaId = "1", DreId = "1", Tipo = (int)TipoResponsavelAtribuicao.PAAI,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new SupervisorEscolaDre()
            {
                SupervisorId = "2", EscolaId = "1", DreId = "1", Tipo = (int)TipoResponsavelAtribuicao.PAAI,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);

            var servicoCadastrarParecerCP = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var parecerDto = ObterPlanoAEECadastroParecerDto();

            await servicoCadastrarParecerCP.Executar(retorno.Items.FirstOrDefault().Id,parecerDto);

            var planoAee = ObterTodos<Dominio.PlanoAEE>();
            planoAee.FirstOrDefault().Situacao = SituacaoPlanoAEE.AtribuicaoPAAI;
            
            var pendencia = ObterTodos<Pendencia>().LastOrDefault();
            var pendenciaPerfil = ObterTodos<PendenciaPerfil>().LastOrDefault();

            pendencia.Id.ShouldBe(pendenciaPerfil.PendenciaId);
            pendenciaPerfil.PerfilCodigo.ShouldBe(PerfilUsuario.CEFAI);
        }
        
         [Fact(DisplayName = "Plano AEE - Deve atribuir automaticamente paai quando a UE tiver um único paai")]
        public async Task Deve_atribuir_automaticamente_paai_quando_a_ue_tiver_um_unico_paai()
        {
            const string LOGIN_USUARIO_PAAI_ATRIBUIDO = "1";
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new Usuario
            {
                Login = LOGIN_USUARIO_PAAI_ATRIBUIDO,CodigoRf = LOGIN_USUARIO_PAAI_ATRIBUIDO,Nome = "Usuario Paai atribuido",
                CriadoPor = SISTEMA_NOME,CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            await InserirNaBase(new SupervisorEscolaDre()
            {
                SupervisorId = "1", EscolaId = "1", DreId = "1", Tipo = (int)TipoResponsavelAtribuicao.PAAI,
                CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF, CriadoEm = DateTime.Now
            });
            
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            var planoAEEPersistenciaDto = ObterPlanoAEEDto();
            var filtroPlanoAeeDto = ObterFiltroPlanoAEEDto();

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);

            var servicoCadastrarParecerCP = ObterServicoCadastrarParecerCPPlanoAEEUseCase();
            var parecerDto = ObterPlanoAEECadastroParecerDto();

            await servicoCadastrarParecerCP.Executar(retorno.Items.FirstOrDefault().Id,parecerDto);

            var planoAee = ObterTodos<Dominio.PlanoAEE>();
            planoAee.FirstOrDefault().Situacao = SituacaoPlanoAEE.ParecerPAAI;

            var pendencias = ObterTodos<Pendencia>();
            pendencias.FirstOrDefault(a=> a.Excluido).Id.ShouldBe(1);
            pendencias.FirstOrDefault(a=> !a.Excluido).Id.ShouldBe(2);

            var pendenciaPerfilusuarios = ObterTodos<PendenciaPerfilUsuario>();
            pendenciaPerfilusuarios.Count.ShouldBe(0);
            
            var pendenciaUsuarios = ObterTodos<Dominio.PendenciaUsuario>();
            var usuarios = ObterTodos<Dominio.Usuario>();
            pendenciaUsuarios.FirstOrDefault().UsuarioId.ShouldBe(usuarios.FirstOrDefault(f=> f.Login.Equals("1")).Id);
        }

        private PlanoAEECadastroParecerDto ObterPlanoAEECadastroParecerDto()
        {
            return new PlanoAEECadastroParecerDto() { Parecer = "Teste Parecer" };
        }

        private FiltroPlanosAEEDto ObterFiltroPlanoAEEDto(SituacaoPlanoAEE situacao = SituacaoPlanoAEE.ParecerCP)
        {
            return new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                Situacao = situacao
            };
        }

        private FiltroPlanosAEEDto ObterFiltroPlanoAEEDtoTurmaAnoAnterior(SituacaoPlanoAEE situacao = SituacaoPlanoAEE.ParecerCP)
        {
            return new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_2,
                UeId = UE_ID_1,
                Situacao = situacao
            };
        }

        private PlanoAEEPersistenciaDto ObterPlanoAEEDto()
        {
            return new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.Validado,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };
        }

        private List<PlanoAEEQuestaoDto> ObterPlanoAeeQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
                { new PlanoAEEQuestaoDto()
                    {   QuestaoId = 2,
                        Resposta = "Teste Resposta",
                        RespostaPlanoId = 1,
                        TipoQuestao = TipoQuestao.Frase
                    }
                };
        }
    }
}