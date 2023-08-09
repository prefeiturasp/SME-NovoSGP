using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresUseCase : AbstractUseCase, IObterAbrangenciaDresUseCase
    {
        public ObterAbrangenciaDresUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Executar(Modalidade? modalidade, int periodo = 0,
            bool consideraHistorico = false, int anoLetivo = 0, string filtro = "")
        {

            //Como essas instancia de query nao possuem parametro definido da pra usar sempre a mesma instancia
            //evitando alocacao de memoria
            var loginTask = mediator
                .Send(ObterLoginAtualQuery.Instance);

            var perfilTask = mediator
                .Send(ObterPerfilAtualQuery.Instance);

            var filtroEhCodigo = false;

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                if (filtro.All(char.IsDigit))
                    filtroEhCodigo = true;
            }

            //tem varios casos de use cases de computacoes distintas que estao aguardando umas as outras para executar
            //nesses casos pode disparar todas ao mesmo tempo ja que sao interdependentes e acumular no final o resultado
            //Um caso é utilizar whenall da plataforma
            await UtilTasks.WhenAll(loginTask, perfilTask);

            //Outro caso é usar o await depois de todas as chamadas assincronas
            var login = await loginTask;
            var perfil = await perfilTask;

            return await mediator
                .Send(new ObterAbrangenciaDresQuery(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo,
                    filtro, filtroEhCodigo));
        }




    }
}