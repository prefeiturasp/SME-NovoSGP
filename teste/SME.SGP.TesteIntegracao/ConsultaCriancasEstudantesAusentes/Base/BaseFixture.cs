using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.ServicosFakes.Rabbit;
using SME.SGP.TesteIntegracao.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SME.SGP.TesteIntegracao.ConsultaCriancasEstudantesAusentes.Base
{
    public abstract class BaseFixture : CollectionFixture
    {
        protected BaseFixture()
        {
            Database.LimparBase();
            IniciarServicos();
            RegistrarFakes(Services);
            BuildServiceProvider();
        }

        protected virtual void RegistrarFakes(IServiceCollection services)
        {
            RegistrarCommandFakes(services);
            RegistrarQueryFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRepositorioCache),
                typeof(RepositorioCacheFake), ServiceLifetime.Scoped));
        }

        protected virtual void RegistrarCommandFakes(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaApiEOLCommand, bool>), typeof(PublicarFilaApiEOLCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>), typeof(PublicarFilaEmLoteSgpCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<SalvarLogViaRabbitCommand, bool>), typeof(SalvarLogViaRabbitCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerComRegistroFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRepositorioCache), typeof(RepositorioCacheFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IServicoAuditoria), typeof(ServicoAuditoriaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IServicoArmazenamento), typeof(ServicoArmazenamentoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRepositorioRegistroFrequenciaAluno), typeof(RepositorioRegistroFrequenciaAlunoFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRepositorioFrequenciaPreDefinida), typeof(RepositorioFrequenciaPreDefinidaFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosAtivosTurmaProgramaPapEolQuery, IEnumerable<AlunosTurmaProgramaPapDto>>), typeof(ObterAlunosAtivosTurmaProgramaPapEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterPerfisPorLoginQuery, PerfisApiEolDto>), typeof(ObterPerfisPorLoginQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorCargoUeQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorCargoUeQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorPerfilDreQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorPerfilDreQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAutenticacaoSemSenhaQuery, AutenticacaoApiEolDto>), typeof(ObterAutenticacaoSemSenhaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQuery, AbrangenciaCompactaVigenteRetornoEOLDTO>), typeof(ObterAbrangenciaCompactaVigenteEolPorLoginEPerfilQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosPorTurmaQuery, IEnumerable<AlunoPorTurmaResposta>>), typeof(ObterAlunosPorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorCodigoEolQuery, AlunoPorTurmaResposta>), typeof(ObterAlunoPorCodigoEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterDadosTurmaEolPorCodigoQuery, DadosTurmaEolDto>), typeof(ObterDadosTurmaEolPorCodigoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PodePersistirTurmaDisciplinaQuery, bool>), typeof(PodePersistirTurmaDisciplinaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioCoreSSOQuery, MeusDadosDto>), typeof(ObterUsuarioCoreSSOQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioFuncionarioQuery, IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterUsuarioFuncionarioQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmasDoProfessorQuery, IEnumerable<ProfessorTurmaReposta>>), typeof(ObterTurmasDoProfessorQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<CarregarDadosAcessoPorLoginPerfilQuery, RetornoDadosAcessoUsuarioSgpDto>), typeof(CarregarDadosAcessoPorLoginPerfilQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterEstruturaInstuticionalVigentePorTurmaQuery, EstruturaInstitucionalRetornoEolDTO>), typeof(ObterEstruturaInstuticionalVigentePorTurmaQueryHandlerFake), ServiceLifetime.Scoped));
        }

        protected virtual void RegistrarQueryFakes(IServiceCollection services)
        {
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery, TurmaParaSyncInstitucionalDto>),
                typeof(ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQueryHandlerFake), ServiceLifetime.Scoped));

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQuery, IEnumerable<ComponenteCurricularEol>>), typeof(ObterComponentesCurricularesEolPorCodigoTurmaLoginEPerfilQueryHandlerFakeServicoEol), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterComponentesCurricularesPorIdsQuery, IEnumerable<DisciplinaDto>>), typeof(ObterComponentesCurricularesPorIdsQueryHandlerFakeServicoEol), ServiceLifetime.Scoped));
        }

        public Task InserirNaBase<T>(IEnumerable<T> objetos) where T : class, new()
        {
            Database.Inserir(objetos);
            return Task.CompletedTask;
        }

        public Task InserirNaBase<T>(T objeto) where T : class, new()
        {
            Database.Inserir(objeto);
            return Task.CompletedTask;
        }

        public Task<long> SalvarNaBase<T>(T objeto) where T : class, new()
        {
            return Database.Salvar(objeto);
        }

        public Task AtualizarNaBase<T>(T objeto) where T : class, new()
        {
            Database.Atualizar(objeto);
            return Task.CompletedTask;
        }

        public Task ExcluirTodos<T>() where T : class, new()
        {
            Database.ExcluirTodos<T>();
            return Task.CompletedTask;
        }

        public Task InserirNaBase(string nomeTabela, params string[] campos)
        {
            Database.Inserir(nomeTabela, campos);
            return Task.CompletedTask;
        }

        public Task InserirNaBase(string nomeTabela, string[] campos, string[] valores)
        {
            Database.Inserir(nomeTabela, campos, valores);
            return Task.CompletedTask;
        }

        public List<T> ObterTodos<T>() where T : class, new()
        {
            return Database.ObterTodos<T>();
        }

        public T ObterPorId<T, K>(K id)
            where T : class, new()
            where K : struct
        {
            return Database.ObterPorId<T, K>(id);
        }
    }
}
