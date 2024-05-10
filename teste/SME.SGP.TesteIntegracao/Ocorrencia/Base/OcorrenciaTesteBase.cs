using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;

namespace SME.SGP.TesteIntegracao.Ocorrencia.Base
{
    public  abstract class OcorrenciaTesteBase : TesteBaseComuns
    {
        protected const int ID_TIPO_INCIDENTE = 1;
        protected const long ALUNO_1 = 1;
        protected const long ALUNO_2 = 2;
        protected const long ALUNO_3 = 3;
        protected const string RF_3333333 = "3333333";
        protected const string RF_4444444 = "4444444";
        protected const string PROFESSOR_INFANTIL = "61E1E074-37D6-E911-ABD6-F81654FE895D";
        protected const string PROFESSOR = "43E1E074-37D6-E911-ABD6-F81654FE895D";

        protected OcorrenciaTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<DeletarArquivoDeRegistroExcluidoCommand, bool>), typeof(DeletarArquivoDeRegistroExcluidoCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<MoverArquivosTemporariosCommand,string>), typeof(MoverArquivosTemporariosCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<RemoverArquivosExcluidosCommand,bool>), typeof(RemoverArquivosExcluidosCommandHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunosEolPorCodigosQuery,IEnumerable<TurmasDoAlunoDto>>), typeof(ObterAlunosEolPorCodigosQueryFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeQuery,IEnumerable<UsuarioEolRetornoDto>>), typeof(ObterFuncionariosPorUeQueryFake), ServiceLifetime.Scoped));
        }

        protected async Task CriarDadosBasicos(Modalidade modalidade = Modalidade.EducacaoInfantil,
                    ModalidadeTipoCalendario modalidadeTipoCalendario = ModalidadeTipoCalendario.Infantil)
        {
            await CriarDadosBasicos(ObterPerfilProfessorInfantil(), modalidade, modalidadeTipoCalendario);
        }

        protected async Task CriarDadosBasicos(string perfil, Modalidade modalidade = Modalidade.EducacaoInfantil,
            ModalidadeTipoCalendario modalidadeTipoCalendario = ModalidadeTipoCalendario.Infantil)
        {
            await CriarDreUePerfil();
            await CriarComponenteCurricular();
            await CriarPeriodoEscolarTodosBimestres();
            await CriarTipoCalendario(modalidadeTipoCalendario);
            CriarClaimUsuario(perfil);
            await CriarUsuarios();
            await CriarTurma(modalidade);
            await CriarTipoOcorrencia();
        }
        
        private async Task CriarPeriodoEscolarTodosBimestres()
        {
            await CriarPeriodoEscolar(DATA_01_02_INICIO_BIMESTRE_1, DATA_25_04_FIM_BIMESTRE_1, BIMESTRE_1);
            await CriarPeriodoEscolar(DATA_02_05_INICIO_BIMESTRE_2, DATA_24_07_FIM_BIMESTRE_2, BIMESTRE_2);
            await CriarPeriodoEscolar(DATA_25_07_INICIO_BIMESTRE_3, DATA_02_10_FIM_BIMESTRE_3, BIMESTRE_3);
            await CriarPeriodoEscolar(DATA_03_10_INICIO_BIMESTRE_4, DATA_22_12_FIM_BIMESTRE_4, BIMESTRE_4);
        }

        private async Task CriarTipoOcorrencia()
        {
            var descricoes = new List<string>
            {
                "Incidente (Brigas, desentendimentos)",
                "Acidente (quedas, machucados)",
                "Alimentação",
                "Como chegou à escola?",
                "Roubo",
                "Furto",
                "Violência contra os professores",
                "Violência contra os funcionários",
                "Violência contra a criança/estudante",
            };

            foreach (var descricao in descricoes)
            {
                await InserirNaBase(new OcorrenciaTipo
                {
                    CriadoPor = "Sistema",
                    CriadoEm = DateTimeExtension.HorarioBrasilia(),
                    CriadoRF = "1",
                    Excluido = false,
                    Descricao = descricao
                });
            }
        }

        protected IExcluirOcorrenciaUseCase ExcluirOcorrenciaUseCase()
        {
            return ServiceProvider.GetService<IExcluirOcorrenciaUseCase>();
        }        
        
        protected IAlterarOcorrenciaUseCase AlterarOcorrenciaUseCase()
        {
            return ServiceProvider.GetService<IAlterarOcorrenciaUseCase>();
        }
        
        protected IInserirOcorrenciaUseCase InserirOcorrenciaUseCase()
        {
            return ServiceProvider.GetService<IInserirOcorrenciaUseCase>();
        }        
        
        protected IListarOcorrenciasUseCase ListarOcorrenciasUseCase()
        {
            return ServiceProvider.GetService<IListarOcorrenciasUseCase>();
        }        
        
        protected IObterOcorrenciaUseCase ObterOcorrenciaUseCase()
        {
            return ServiceProvider.GetService<IObterOcorrenciaUseCase>();
        }        
        
        protected IObterOcorrenciasPorAlunoUseCase ObterOcorrenciasPorAlunoUseCase()
        {
            return ServiceProvider.GetService<IObterOcorrenciasPorAlunoUseCase>();
        }

    }
}