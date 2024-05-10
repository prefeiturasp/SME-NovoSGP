using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SME.SGP.Aplicacao;
using SME.SGP.TesteIntegracao.Setup;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.RegistroIndividual.ServicosFakes;
using SME.SGP.TesteIntegracao.ServicosFakes;

namespace SME.SGP.TesteIntegracao.RegistroIndividual
{
    public abstract class RegistroIndividualTesteBase : TesteBaseComuns
    {
        private const string NOME_TABELA_SUGESTAO = "registro_individual_sugestao (mes, descricao)";
        protected const string DESCRICAO_REGISTRO_INDIVIDUAL = "Descrição do registro individual";
        protected const long COMPONENTE_CURRICULAR_CODIGO_512 = 512;
        protected readonly DateTime DATA_DESISTENCIA_ALUNO_5_REGISTRO_INDIVIDUAL = DateTimeExtension.HorarioBrasilia().Date.AddDays(-5);


        public RegistroIndividualTesteBase(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
        
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<MoverArquivosTemporariosCommand, string>),
                 typeof(MoverArquivosTemporariosCommandHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarAtualizacaoPendenciaRegistroIndividualCommand>),
                typeof(PublicarAtualizacaoPendenciaRegistroIndividualCommandHandlerFake), ServiceLifetime.Scoped));
            
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoPorTurmaAlunoCodigoQuery,AlunoPorTurmaResposta>),
                typeof(ObterAlunoPorTurmaAlunoCodigoQueryHandlerFake), ServiceLifetime.Scoped));
        }
        protected IInserirRegistroIndividualUseCase ObterServicoInserirRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IInserirRegistroIndividualUseCase>();
        }
        protected IAlterarRegistroIndividualUseCase ObterServicoAlterarRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IAlterarRegistroIndividualUseCase>();
        }

        protected IExcluirRegistroIndividualUseCase ObterServicoExcluirRegistroUseCase()
        {
            return ServiceProvider.GetService<IExcluirRegistroIndividualUseCase>();
        }

        protected IObterRegistroIndividualPorAlunoDataUseCase ObterServicoListarRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IObterRegistroIndividualPorAlunoDataUseCase>();
        }

        protected IObterSugestaoTopicoRegistroIndividualPorMesUseCase ObterServicoSugestaoRegistroIndividualUseCase()
        {
            return ServiceProvider.GetService<IObterSugestaoTopicoRegistroIndividualPorMesUseCase>();
        }
        
        protected async Task CriarDadosBasicos(FiltroRegistroIndividualDto filtroRegistroIndividualDto)
        {
            await CriarTipoCalendario(filtroRegistroIndividualDto.TipoCalendario);

            await CriarDreUePerfil();
            
            if (!filtroRegistroIndividualDto.NaoCriarPeriodosEscolares)
                await CriarPeriodoEscolarCustomizadoQuartoBimestre(!filtroRegistroIndividualDto.BimestreEncerrado);

            await CriarComponenteCurricular();

            CriarClaimUsuario(filtroRegistroIndividualDto.Perfil);

            await CriarUsuarios();

            await CriarTurma(filtroRegistroIndividualDto.Modalidade, filtroRegistroIndividualDto.EhAnoAnterior);

            if (filtroRegistroIndividualDto.CriarPeriodoReabertura)
                await CriarPeriodoReabertura(filtroRegistroIndividualDto.TipoCalendarioId);
        }
        
        private async Task CriarTurma(Modalidade modalidade, bool ehAnoAnterior = false)
        {
            await InserirNaBase(new Dominio.Turma
            {
                UeId = 1,
                Ano = TURMA_ANO_2,
                CodigoTurma = TURMA_CODIGO_1,
                Historica = ehAnoAnterior,
                ModalidadeCodigo = modalidade,
                AnoLetivo = ehAnoAnterior ? DateTimeExtension.HorarioBrasilia().AddYears(-1).Year : DateTimeExtension.HorarioBrasilia().Year,
                Semestre = SEMESTRE_1,
                Nome = TURMA_NOME_1,
                TipoTurma = TipoTurma.Regular
            });
        }

        protected Dictionary<int, string> ObterDicionarioDeSugestao()
        {
            var sugestaoDeTopicosDic = new Dictionary<int, string>();
            sugestaoDeTopicosDic.Add(2, "Momento de adaptação e acolhimento. Como foi ou está sendo este processo para a criança e a família?");
            sugestaoDeTopicosDic.Add(3, "Como a criança brinca e interage no parque, área externa e outros espaços da unidade?");
            sugestaoDeTopicosDic.Add(4, "Como as crianças se relacionam consigo mesmas e com o grupo?");
            sugestaoDeTopicosDic.Add(5, "Como a criança responde às intervenções do professor(a)?");
            sugestaoDeTopicosDic.Add(6, "Quais os maiores interesses da criança? Como está a relação da família com a escola?");
            sugestaoDeTopicosDic.Add(7, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(8, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(9, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(10, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(11, "Evidências de oferta e evidências de aprendizagem.");
            sugestaoDeTopicosDic.Add(12, "Evidências de oferta e evidências de aprendizagem.");

            return sugestaoDeTopicosDic;
        }

        protected async Task CriarSugestaoDeTopicos(Dictionary<int, string> sugestaoDeTopicosDic)
        {
            foreach (var chave in sugestaoDeTopicosDic.Keys)
            {
                await InserirNaBase(NOME_TABELA_SUGESTAO, chave.ToString(), "'" + sugestaoDeTopicosDic[chave] + "'");
            }
        }

    }
}
