using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.TesteIntegracao.PendenciaGeral.ServicosFake;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.TesteIntegracao.PendenciaGeral
{
    public class Ao_remover_pendencia_calendario_ano_aterior : TesteBaseComuns
    {
        public Ao_remover_pendencia_calendario_ano_aterior(CollectionFixture collectionFixture) : base(collectionFixture)
        {
        }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQuery, int>), typeof(ObterAnoLetivoUltimoTipoCalendarioPorAnoReferenciaQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterTodasUesIdsQuery, IEnumerable<long>>), typeof(ObterTodasUesIdsQueryHandlerFake), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Pendências Calendário - Replicar Paramento Ano Anterior")]
        public async Task Ao_replicar_parametro_ano_anterior()
        {
            await CriarBaseReplicarParametro();
            var useCase = ServiceProvider.GetService<IReplicarParametrosAnoAnteriorUseCase>();
            var dtoParametro = new FiltroInclusaoParametrosAnoAnterior() {AnoLetivo = DateTimeExtension.HorarioBrasilia().Year, ModalidadeTipoCalendario = ModalidadeTipoCalendario.Infantil};
            var mensagemParaPublicar = new MensagemRabbit() {Mensagem = JsonConvert.SerializeObject(dtoParametro)};
            var retornoUseCase = await useCase.Executar(mensagemParaPublicar);
            retornoUseCase.ShouldBeTrue();
            var obterTodosParam = ObterTodos<ParametrosSistema>();
            obterTodosParam.Count.ShouldBeEquivalentTo(2);
        }
        
        [Fact(DisplayName = "Pendências Calendário - Exclusão de Pendencias Calendario")]
        public async Task Ao_Excluir_Pendencia_Calendario_Ano_Anterior_Calendario_PorUe()
        {
            await CriarBaseReplicarParametro();
            await CriarBaseReplicarExcluirCalendarioAnoAnteriorCalendario();
            var pendenciasAntesDaExlusao = ObterTodos<Pendencia>().Where(x => !x.Excluido);
            pendenciasAntesDaExlusao.Count().ShouldBeEquivalentTo(3);
            
            var useCase = ServiceProvider.GetService<IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase>();

            var pendenciasParaExcluir = pendenciasAntesDaExlusao.Select(x => x.Id).ToArray();
            
            var mensagemParaPublicar = new MensagemRabbit() {Mensagem = JsonConvert.SerializeObject(pendenciasParaExcluir)};
            var retornoUseCase = await useCase.Executar(mensagemParaPublicar);
            retornoUseCase.ShouldBeTrue();
            
            var pendenciasDepoisDaExlusao = ObterTodos<Pendencia>().Where(x => x.Excluido);
            pendenciasDepoisDaExlusao.Count().ShouldBeEquivalentTo(3);
        }

        private async Task CriarBaseReplicarParametro()
        {
            var anoAnterior = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year;
            var anoAtual = DateTimeExtension.HorarioBrasilia().Year;
            await InserirNaBase(new TipoCalendario()
            {
                AnoLetivo = anoAtual,
                Excluido = false,
                Migrado = false,
                Modalidade = ModalidadeTipoCalendario.Infantil,
                Nome = $"Calendário Escolar de {anoAtual}",
                Periodo = Periodo.Anual,
                Situacao = true,
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
            await InserirNaBase(new ParametrosSistema()
            {
                Ano = anoAnterior,
                Ativo = true,
                Tipo = TipoParametroSistema.GerarPendenciaDiasLetivosInsuficientes,
                Descricao = $"Calendário Escolar de {anoAnterior}",
                Nome = $"Calendário Escolar de {anoAnterior}",
                Valor = "1",
                CriadoEm = DateTime.Now,
                CriadoPor = "Sistema",
                CriadoRF = "1"
            });
        }

        private async Task CriarBaseReplicarExcluirCalendarioAnoAnteriorCalendario()
        {
            await InserirNaBase(new Pendencia()
            {
                Tipo = TipoPendencia.AulaNaoLetivo ,
                Descricao = "Aulas criadas em dias não letivos",
                Titulo = "Aulas criadas em dias não letivos",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 03, 01)
            });
            await InserirNaBase(new Pendencia()
            {
                Tipo = TipoPendencia.CalendarioLetivoInsuficiente ,
                Descricao = "Calendário com dias letivos abaixo do permitido",
                Titulo = "Calendário com dias letivos abaixo do permitido",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 03, 01)
            });
            await InserirNaBase(new Pendencia()
            {
                Tipo = TipoPendencia.CadastroEventoPendente  ,
                Descricao = "Cadastro de eventos pendente",
                Titulo = "Cadastro de eventos pendente",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().AddYears(-1).Year, 03, 01)
            });
            await InserirNaBase(new Dre()
            {
                Nome = "Dre Teste",
                CodigoDre = "11",
                Abreviacao = "DT"
            });
            await InserirNaBase(new Ue()
            {
                Nome = "Ue Teste",
                DreId = 1,
                TipoEscola = TipoEscola.CEU,
                CodigoUe = "22"
            });
            await InserirNaBase(new Dominio.Turma()
            {
                Nome = "7F",
                CodigoTurma = "111",
                Ano = "1",
                AnoLetivo = DateTimeExtension.HorarioBrasilia().AddYears(-1).Year,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                ModalidadeCodigo = Modalidade.EducacaoInfantil,
                UeId = 1
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
                TurmaId = "111",
                CriadoPor = "",
                CriadoRF = "",
                CriadoEm = new DateTime(DateTimeExtension.HorarioBrasilia().Year, 03, 01),
            });

            await InserirNaBase(new Dominio.PendenciaAula()
            {
                Id = 1,
                AulaId = 1,
                PendenciaId = 1
            });
            await InserirNaBase(new Dominio.PendenciaAula()
            {
                Id = 2,
                AulaId = 1,
                PendenciaId = 2
            });
            await InserirNaBase(new Dominio.PendenciaAula()
            {
                Id = 3,
                AulaId = 1,
                PendenciaId = 3
            });
        }
    }
}