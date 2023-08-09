using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ObterModalidadesPorAnoUseCase : IObterModalidadesPorAnoUseCase
    {
        private readonly IMediator mediator;

        public ObterModalidadesPorAnoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> Executar(int anoLetivo, bool consideraHistorico, bool consideraNovasModalidades)
        {
            //Como essas instancia de query nao possuem parametro definido da pra usar sempre a mesma instancia
            //evitando alocacao de memoria para esses casos
            var loginTask = mediator
                .Send(ObterLoginAtualQuery.Instance);

            //Como essas instancia de query nao possuem parametro definido da pra usar sempre a mesma instancia
            //evitando alocacao de memoria para esses casos
            var perfilTask = mediator
                .Send(ObterPerfilAtualQuery.Instance);

            var modalidadesQueSeraoIgnoradasTask = mediator
                .Send(new ObterNovasModalidadesPorAnoQuery(anoLetivo, consideraNovasModalidades));

            //tem varios casos de use cases de computacoes distintas que estao aguardando umas as outras para executar com awaits encadeados
            //nesses casos pode disparar todas ao mesmo tempo ja que sao interdependentes e acumular no final o resultado
            //Um caso é utilizar whenall da plataforma
            await UtilTasks.WhenAll(loginTask, perfilTask,modalidadesQueSeraoIgnoradasTask);

            //Outro caso é usar o await depois de todas as chamadas assincronas serem feitas, pra todos os efeitos nao da pra ter certeza se nesses
            //casos que o comportamento bloqueia da mesma maneira na sequencia que não é desejavel e ainda nesses casos seriam 3 chamadas de state machine
            //contra apenas uma da utilizacao de whenAll + Result
            var login = loginTask.Result; //exemplo com Result que nesse caso é garantido estar computado e nao cria state machine
            var perfil = await perfilTask; //exemplo com await ja Completed. Nesse caso vai criar state machine que vai executar direto, so fica codigo compilado a mais so, nao tem punicao.
            var modalidadesQueSeraoIgnoradas = await modalidadesQueSeraoIgnoradasTask;

            return await mediator
                .Send(new ObterModalidadesPorAnoQuery(anoLetivo, consideraHistorico, login, perfil, modalidadesQueSeraoIgnoradas));
        }
    }
}