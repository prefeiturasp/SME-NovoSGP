using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shouldly;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake;
using SME.SGP.TesteIntegracao.Aula.DiarioBordo.ServicosFakes;
using SME.SGP.TesteIntegracao.CartaIntencoes.ServicosFakes;
using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao
{
    public class Ao_abrir_a_carta_de_intencoes : TesteBaseComuns
    {
        private DateTime dataAtual = DateTime.Now.Date;
        public Ao_abrir_a_carta_de_intencoes(CollectionFixture collectionFixture) : base(collectionFixture)
        { }

        protected override void RegistrarFakes(IServiceCollection services)
        {
            base.RegistrarFakes(services);
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>), typeof(ObterAlunoEnderecoEolQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessorTitularPorTurmaEComponenteCurricularQuery, ProfessorTitularDisciplinaEol>), typeof(ObterProfessorTitularPorTurmaEComponenteCurricularQueryHandlerFakeCartaIntencoes), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterProfessoresTitularesDasTurmasQuery, IEnumerable<ProfessorTitularDisciplinaEol>>), typeof(ObterProfessoresTitularesDasTurmasQueryHandlerFakeCartaIntencoes), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuariosNotificarCartaIntencoesObservacaoQuery, IEnumerable<UsuarioNotificarCartaIntencoesObservacaoDto>>), typeof(ObterUsuariosNotificarCartaIntencoesObservacaoQueryHandlerFake), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterUsuarioLogadoQuery, Usuario>), typeof(ObterUsuarioLogadoHandlerFakeCartaIntencoes), ServiceLifetime.Scoped));
            services.Replace(new ServiceDescriptor(typeof(IRequestHandler<ObterFuncionariosPorUeECargoQuery, IEnumerable<FuncionarioDTO>>), typeof(ObterFuncionariosPorUeECargoQueryHandlerFakeCartaIntencoes), ServiceLifetime.Scoped));
        }

        [Fact(DisplayName = "Carta de Intenções - Deve retornar somente um usuário titular no caso de ter a possibilidade de ter 2 titulares ao notificar")]
        public async Task Deve_obter_somente_um_usuario_titular_podendo_manter_a_possibilidade_de_ter_2_titulares()
        {
            await CriarDados();

            var mediator = ServiceProvider.GetService<IMediator>();
            var retorno = await mediator.Send(new ObterCartaIntencoesNotificacaoQuery(1, "512"));

            retorno.ShouldNotBeNull();
            retorno.Count().ShouldBe(1);
            retorno.FirstOrDefault().Nome.ShouldBe("Professor Teste");
        }

        private async Task CriarDados()
        {
            await InserirNaBase(new Dre()
            {
                Id = 1,
                CodigoDre = "1",
                DataAtualizacao = dataAtual
            });

            await InserirNaBase(new Ue()
            {
                Id = 1,
                CodigoUe = "1",
                DataAtualizacao = dataAtual,
                DreId = 1
            });

            await InserirNaBase(new Dominio.Turma()
            {
                Id = 1,
                CodigoTurma = "1",
                DataAtualizacao = dataAtual,
                TipoTurma = Dominio.Enumerados.TipoTurma.Regular,
                UeId = 1,
                AnoLetivo = dataAtual.Year,
                ModalidadeCodigo = Modalidade.EducacaoInfantil
            });

        }
    }
}
