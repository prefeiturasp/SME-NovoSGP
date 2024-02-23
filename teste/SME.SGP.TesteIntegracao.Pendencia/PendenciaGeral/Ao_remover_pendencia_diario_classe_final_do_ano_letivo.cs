using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dados;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaGeral
{
    public class Ao_remover_pendencia_diario_classe_final_do_ano_letivo : TesteBaseComuns
    {
        public Ao_remover_pendencia_diario_classe_final_do_ano_letivo(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }
        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);

            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<PublicarFilaSgpCommand, bool>), typeof(PublicarFilaSgpCommandHandlerFakeRemoverPendencia), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Pendência Frequência - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_pendencia_aulas_sem_frequencia_registrada_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.Frequencia);
            await CriarPendenciaAula();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }

        [Fact(DisplayName = "Pendência Plano de Aula - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_aulas_sem_plano_de_aula_registrado_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.PlanoAula);
            await CriarPendenciaAula();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }

        [Fact(DisplayName = "Pendência Diário de Bordo - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_aulas_sem_diario_de_bordo_registrado_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.DiarioBordo);
            await CriarUsuarios();
            await CriaPendenciaDiarioBordo();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }

        [Fact(DisplayName = "Pendência Avaliações - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_avaliacoes_sem_nota_registrada_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.Avaliacao);
            await CriarPendenciaAula();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }

        [Fact(DisplayName = "Pendência Registro Individual - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_criancas_com_registro_individual_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.AusenciaDeRegistroIndividual);
            await CriaPendenciaIndividual();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Pendência Devolutivas - Deve permitir excluir logicamente a pendência ao final do ano letivo")]
        public async Task Ao_remover_criancas_com_devolutivas_final_do_ano_letivo()
        {
            await CriarBase(TipoPendencia.Devolutiva);
            await CriaPendenciaDevolutiva();

            var useCase = ServiceProvider.GetService<IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase>();
            var filtro = new FiltroRemoverPendenciaFinalAnoLetivoDto(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 1, "1");
            var mensagemParaPublicar = new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) };

            await useCase.Executar(mensagemParaPublicar);

            var pendencias = ObterTodos<Dominio.Pendencia>();

            pendencias.FirstOrDefault().Excluido.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Pendência Geral - Não deve permitir excluir logicamente as pendências em função do ano ser abaixo de 2014")]
        public async Task Ao_remover_pendencia_no_final_do_ano_com_ano_abaixo_2014()
        {
            var useCase = ServiceProvider.GetService<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(2010));
            retorno.ShouldBeFalse();
        }
        
        [Fact(DisplayName = "Pendência Geral - Deve permitir excluir logicamente as pendências em função do ano ser válido >= 2014 e < atual")]
        public async Task Ao_remover_pendencia_no_final_do_ano_com_ano_2014()
        {
            var useCase = ServiceProvider.GetService<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(2014));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Pendência Geral - Deve permitir excluir logicamente as pendências em função do ano ser válido >= 2014 e < atual")]
        public async Task Ao_remover_pendencia_no_final_do_ano_com_ano_anterior_ao_atual()
        {
            var useCase = ServiceProvider.GetService<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year));
            retorno.ShouldBeTrue();
        }
        
        [Fact(DisplayName = "Pendência Geral - Não deve permitir excluir logicamente as pendências do ano atual")]
        public async Task Ao_remover_pendencia_no_final_do_ano_com_ano_atual()
        {
            var useCase = ServiceProvider.GetService<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(DateTimeExtension.HorarioBrasilia().Year));
            retorno.ShouldBeFalse();
        }

        [Fact(DisplayName = "Pendência Geral - Não deve permitir excluir logicamente as pendências de anos futuros")]
        public async Task Ao_remover_pendencia_no_final_do_ano_para_anos_futuros()
        {
            var useCase = ServiceProvider.GetService<IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase>();
            var retorno = await useCase.Executar(new MensagemRabbit(DateTimeExtension.HorarioBrasilia().AddYears(1).Year));
            retorno.ShouldBeFalse();
        }

        private async Task CriarBase(TipoPendencia tipoPendencia)
        {
            await CriarDreUePerfil();
            await CriarTurma(Modalidade.EducacaoInfantil, true);
            await CriarComponenteCurricular();
            await CriarTipoCalendario(ModalidadeTipoCalendario.Infantil, true);

            await InserirNaBase(new Dominio.Pendencia()
            {
                Tipo = tipoPendencia,
                Situacao = SituacaoPendencia.Pendente,
                Descricao = "pendência",
                Titulo = "pendência",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01)
            });

            await InserirNaBase(new Dominio.Aula()
            {
                AulaCJ = false,
                DataAula = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 01, 03),
                ProfessorRf = "Sistema",
                DisciplinaId = "512",
                Excluido = false,
                TipoCalendarioId = 1,
                RecorrenciaAula = RecorrenciaAula.AulaUnica,
                Quantidade = 1,
                TipoAula = TipoAula.Normal,
                UeId = "1",
                TurmaId = "1",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01),
            });
        }

        private async Task CriarPendenciaAula()
        {
            await InserirNaBase(new Dominio.PendenciaAula()
            {
                Id = 1,
                AulaId = 1,
                PendenciaId = 1
            });
        }

        private async Task CriaPendenciaDiarioBordo()
        {
            await InserirNaBase(new Dominio.PendenciaDiarioBordo()
            {
                Id = 1,
                AulaId = 1,
                PendenciaId = 1,
                ComponenteId = COMPONENTE_CURRICULAR_PORTUGUES_ID_138,
                ProfessorRf = USUARIO_PROFESSOR_CODIGO_RF_2222222,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01)
            });
        }

        private async Task CriaPendenciaIndividual()
        {
            await InserirNaBase(new PendenciaRegistroIndividual()
            {
                Id = 1,
                PendenciaId = 1,
                TurmaId = 1,
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01)
            });
        }
        
        private async Task CriaPendenciaDevolutiva()
        {
            await InserirNaBase(new Dominio.PendenciaDevolutiva()
            {
                TurmaId = 1,
                ComponenteCurricularId = COMPONENTE_CURRICULAR_512,
                PedenciaId = 1
            });
        }
    }
}
