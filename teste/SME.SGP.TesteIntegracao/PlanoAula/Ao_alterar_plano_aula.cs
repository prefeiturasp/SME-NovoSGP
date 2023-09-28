using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PlanoAula.Base;
using SME.SGP.TesteIntegracao.PlanoAula.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.PlanoAula
{
    public class Ao_alterar_plano_aula : PlanoAulaTesteBase
    {
        public Ao_alterar_plano_aula(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        private const string NOVA_DESCRICAO_PlANO_AULA = "<p><span>Objetivos específicos e desenvolvimento da aula EDITADO</span></p>";

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            RegistraFake(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A));
            RegistraFake(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake));
            RegistraFake(typeof(IRequestHandler<RemoverArquivosExcluidosCommand, bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake));
            RegistraFake(typeof(IRequestHandler<ObterAbrangenciaTurmaQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaTurmaQueryFake));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaPorTurmaEConsideraHistoricoQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaPorTurmaEConsideraHistoricoQueryHandlerFakeFundamental6A), ServiceLifetime.Scoped));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQuery, bool>), typeof(ObterUsuarioPossuiPermissaoNaTurmaEDisciplinaQueryHandlerComPermissaoFake), ServiceLifetime.Scoped));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverArquivosExcluidosCommand, bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake), ServiceLifetime.Scoped));
            //services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaTurmaQuery, AbrangenciaFiltroRetorno>), typeof(ObterAbrangenciaTurmaQueryFake), ServiceLifetime.Scoped));
        }

        [Fact]
        public async Task Deve_alterar_plano_componentes_com_objetivos()
        {
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
        }

        [Fact]
        public async Task Deve_alterar_plano_componentes_sem_objetivos()
        {
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlteradoSemObetivos(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count.ShouldBe(0);
        }

        [Fact]
        public async Task Deve_alterar_plano_pelo_CP()
        {
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilCP(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(0);
        }
        
        [Fact]
        public async Task Deve_alterar_plano_com_objetivos_excluindo_objetivos_parcialmente()
        {
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);
            planoAulaAlteradoDto.ObjetivosAprendizagemComponente =
                planoAulaAlteradoDto.ObjetivosAprendizagemComponente.Where(w => w.Id == 3).ToList();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();
            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.ShouldNotBeNull();
            objetivoAprendizagemAulas.Count.ShouldBe(3);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(2);
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(1);
        }
        
        [Fact]
        public async Task Deve_alterar_plano_com_objetivos_excluindo_objetivos_totalmente()
        {
            var salvarPlanoAulaUseCase = ObterServicoSalvarPlanoAulaUseCase();

            await CriarDadosBasicos(new FiltroPlanoAula()
            {
                Bimestre = BIMESTRE_2,
                Modalidade = Modalidade.Fundamental,
                Perfil = ObterPerfilProfessor(),
                QuantidadeAula = 1,
                DataAula = DateTimeExtension.HorarioBrasilia().Date,
                DataInicio = DATA_02_05_INICIO_BIMESTRE_2,
                DataFim = DATA_24_07_FIM_BIMESTRE_2,
                CriarPeriodoEscolarBimestre = false,
                TipoCalendario = ModalidadeTipoCalendario.FundamentalMedio,
                ComponenteCurricularCodigo = COMPONENTE_LINGUA_PORTUGUESA_ID_138,
                TipoCalendarioId = TIPO_CALENDARIO_1,
            });

            await InserirNaBase(new Dominio.PlanoAula()
            {
                AulaId = AULA_ID_1,
                Descricao = "Descrição do plano de aula",
                RecuperacaoAula = "Recuperação aula do plano de aula",
                LicaoCasa = "Lição de casa do plano de aula",
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 1,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });
            
            await InserirNaBase(new Dominio.ObjetivoAprendizagemAula()
            {
                PlanoAulaId = 1,
                ComponenteCurricularId = long.Parse(COMPONENTE_LINGUA_PORTUGUESA_ID_138),
                ObjetivoAprendizagemId = 2,
                CriadoEm = DateTime.Now, CriadoPor = SISTEMA_NOME, CriadoRF = SISTEMA_CODIGO_RF
            });

            var listaPlanoAulaPersistido = ObterTodos<SME.SGP.Dominio.PlanoAula>();

            var planoAulaPersistido = listaPlanoAulaPersistido.FirstOrDefault();

            var planoAulaAlteradoDto = ObterPlanoAulaAlterado(planoAulaPersistido.Id, NOVA_DESCRICAO_PlANO_AULA);
            planoAulaAlteradoDto.ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>();

            var retorno = await salvarPlanoAulaUseCase.Executar(planoAulaAlteradoDto);
            retorno.ShouldNotBeNull();
            retorno.Id.ShouldBe(1);

            var listaPlanoAulaEditado = ObterTodos<SME.SGP.Dominio.PlanoAula>();
            listaPlanoAulaEditado.ShouldNotBeNull();
            listaPlanoAulaEditado.FirstOrDefault().Id.ShouldBe(planoAulaPersistido.Id);
            listaPlanoAulaEditado.FirstOrDefault().Descricao.ShouldNotBe(planoAulaPersistido.Descricao);
            
            var objetivoAprendizagemAulas = ObterTodos<Dominio.ObjetivoAprendizagemAula>();
            objetivoAprendizagemAulas.ShouldNotBeNull();
            objetivoAprendizagemAulas.Count.ShouldBe(2);
            objetivoAprendizagemAulas.Count(w=> w.Excluido).ShouldBe(2);
            objetivoAprendizagemAulas.Count(w=> !w.Excluido).ShouldBe(0);
        }

        private PlanoAulaDto ObterPlanoAula()
        {
            return new PlanoAulaDto()
            {
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 1
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 2
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 3
                    },
                },
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaSemObjetivos()
        {
            return new PlanoAulaDto()
            {
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = "<p><span>Objetivos específicos e desenvolvimento da aula</span></p>",
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = null,
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaAlterado(long id, string descricao)
        {
            return new PlanoAulaDto()
            {
                Id = id,
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = descricao,
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = new List<ObjetivoAprendizagemComponenteDto>()
                {
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 1
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 2
                    },
                    new()
                    {
                        ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                        Id = 3
                    },
                },
                RecuperacaoAula = null
            };
        }

        private PlanoAulaDto ObterPlanoAulaAlteradoSemObetivos(long id, string descricao)
        {
            return new PlanoAulaDto()
            {
                Id = id,
                ComponenteCurricularId = COMPONENTE_TERRITORIO_SABER_EXP_PEDAG_ID_1214,
                ConsideraHistorico = false,
                AulaId = AULA_ID_1,
                Descricao = descricao,
                LicaoCasa = null,
                ObjetivosAprendizagemComponente = null,
                RecuperacaoAula = null
            };
        }
    }
}