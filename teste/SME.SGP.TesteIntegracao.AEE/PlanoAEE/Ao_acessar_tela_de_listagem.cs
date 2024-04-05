using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.AEE.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.PlanoAEE.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAEE
{
    public class Ao_acessar_tela_de_listagem : PlanoAEETesteBase
    {
        public Ao_acessar_tela_de_listagem(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaRegularESrmPorAlunoQuery, IEnumerable<TurmasDoAlunoDto>>), typeof(ObterTurmaRegularESrmPorAlunoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUEsPorDREQuery, IEnumerable<AbrangenciaUeRetorno>>), typeof(ObterUEsPorDREQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Plano AEE - Deve exibir o historico ao selecionar uma turma de 2021")]
        public async Task Deve_exibir_historico_ao_selecionar_turma_de_2021()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await CriarTurma(Modalidade.Fundamental, TURMA_ANO_4, TURMA_CODIGO_4, TipoTurma.Regular, true);
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_4.ToString(),
                TurmaId = TURMA_ID_4,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_4,
                UeId = UE_ID_1
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            var planoAee = retorno.Items.FirstOrDefault();
            planoAee.Id.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar por uma turma de com anterior")]
        public async Task Deve_filtrar_por_turma_ano_anterior()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            await CriarTurma(Modalidade.Fundamental, TURMA_ANO_4, TURMA_CODIGO_4, TipoTurma.Regular, true);
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_4.ToString(),
                TurmaId = TURMA_ID_4,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);
            var opa = ObterTodos<SME.SGP.Dominio.PlanoAEE>();
            
            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_4,
                UeId = UE_ID_1,
                Situacao = null
            };
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar por uma turma com ano atual")]
        public async Task Deve_filtrar_por_turma_ano_atual()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();

            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            var tipoEscola = ObterTodos<TipoEscolaEol>();

            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = TURMA_ID_1,
                UeId = UE_ID_1,
                Situacao = null
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar pelo nome do estudante")]
        public async Task Deve_filtrar_por_nome_do_estudante()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
            
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = 0,
                UeId = UE_ID_1,
                Situacao = null
            };
            
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Plano AEE - Deve filtrar pela situacao")]
        public async Task Deve_filtrar_por_situacao()
        {
            var servicoCadastrarPlanoAee = ObterServicoSalvarPlanoAEEUseCase();
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });
           
            var planoAEEPersistenciaDto = new PlanoAEEPersistenciaDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                TurmaCodigo = TURMA_ID_1.ToString(),
                TurmaId = TURMA_ID_1,
                ResponsavelRF = SISTEMA_CODIGO_RF,
                Questoes = ObterPlanoAeeQuestoes()
            };

            await servicoCadastrarPlanoAee.Executar(planoAEEPersistenciaDto);

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                AlunoCodigo = CODIGO_ALUNO_1,
                DreId = DRE_ID_1,
                TurmaId = 0,
                UeId = UE_ID_1,
                Situacao = SituacaoPlanoAEE.ParecerCP
            };

            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.Count().ShouldBeGreaterThan(0);
        }

        [Fact]
        public async Task Ao_filtrar_encaminhamento_todas_ue()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TurmasMesmaUe = false
            });
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_2, ANO_LETIVO_ANO_ATUAL, false);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_3, ANO_LETIVO_ANO_ATUAL, false);

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_2,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_3,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNumero = 2,
                AlunoNome = "Nome do aluno 2",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 2,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 2,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                UeId = 0,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                ConsideraHistorico = false
            };

            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(2);
            retorno.Items.ToList().Exists(plano => plano.Ue.Equals("EMEF UE 2")).ShouldBeTrue();
            retorno.Items.ToList().Exists(plano => plano.Ue.Equals("EMEF UE 2")).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_nao_filtrar_nenhuma_ue_encaminhamento()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                TurmasMesmaUe = false
            });
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_3, TipoTurma.Regular, UE_ID_2, ANO_LETIVO_ANO_ATUAL, false);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_2, TipoTurma.Regular, UE_ID_3, ANO_LETIVO_ANO_ATUAL, false);
            await CriarTurma(Modalidade.Fundamental, "1", TURMA_CODIGO_1, TipoTurma.Regular, UE_ID_1, ANO_LETIVO_ANO_ATUAL, false);

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_2,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_3,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNumero = 2,
                AlunoNome = "Nome do aluno 2",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 2,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.ParecerCP,
                AlunoCodigo = ALUNO_CODIGO_3,
                AlunoNumero = 4,
                AlunoNome = "Nome do aluno 3",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 3,
                Numero = 4,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 2,
                UeId = -99,
                AnoLetivo = ANO_LETIVO_ANO_ATUAL,
                ConsideraHistorico = false
            };

            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(2);
            retorno.Items.ToList().Exists(plano => plano.Ue.Equals("EMEF UE 2")).ShouldBeTrue();
        }

        [Fact]
        public async Task Ao_filtrar_encaminhamento_sem_exibir_encerrados()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.Encerrado,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });


            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.EncerradoAutomaticamente,
                AlunoCodigo = ALUNO_CODIGO_2,
                AlunoNumero = 2,
                AlunoNome = "Nome do aluno 2",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 2,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 1
            };
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.Count().ShouldBe(0);
        }


        [Fact]
        public async Task Ao_filtrar_encaminhamento_pelo_responsavel()
        {
            await CriarDadosBasicos(new FiltroPlanoAee()
            {
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            });

            await InserirNaBase(new Dominio.PlanoAEE()
            {
                TurmaId = TURMA_ID_1,
                Situacao = SituacaoPlanoAEE.AtribuicaoPAAI,
                AlunoCodigo = CODIGO_ALUNO_1,
                AlunoNumero = 1,
                AlunoNome = "Nome do aluno 1",
                Questoes = new List<PlanoAEEQuestao>(),
                ResponsavelId = USUARIO_ID_1,
                ResponsavelPaaiId = USUARIO_ID_2,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            await InserirNaBase(new PlanoAEEVersao()
            {
                PlanoAEEId = 1,
                Numero = 1,
                CriadoPor = SISTEMA_NOME,
                CriadoRF = SISTEMA_CODIGO_RF,
                CriadoEm = DateTime.Now
            });

            var filtroPlanoAeeDto = new FiltroPlanosAEEDto()
            {
                DreId = 1,
                ResponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                PaaiReponsavelRf = USUARIO_PROFESSOR_CODIGO_RF_1111111,
                UeId = 1
            };
            var servicoObterPlanoAEE = ObterServicoObterPlanosAEEUseCase();
            var retorno = await servicoObterPlanoAEE.Executar(filtroPlanoAeeDto);
            retorno.ShouldNotBeNull();
            retorno.Items.ShouldNotBeNull();
            retorno.Items.ToList().Exists(plano => plano.RfReponsavel == USUARIO_PROFESSOR_CODIGO_RF_2222222).ShouldBeTrue();
            retorno.Items.ToList().Exists(plano => plano.RfPaaiReponsavel == USUARIO_PROFESSOR_CODIGO_RF_1111111).ShouldBeTrue();
        }

        private List<PlanoAEEQuestaoDto> ObterPlanoAeeQuestoes()
        {
            return new List<PlanoAEEQuestaoDto>()
                { new PlanoAEEQuestaoDto
                    {   QuestaoId = 2,
                        Resposta = "Teste Resposta",
                        RespostaPlanoId = 1,
                        TipoQuestao = TipoQuestao.Frase
                    }
                };
        }
    }
}