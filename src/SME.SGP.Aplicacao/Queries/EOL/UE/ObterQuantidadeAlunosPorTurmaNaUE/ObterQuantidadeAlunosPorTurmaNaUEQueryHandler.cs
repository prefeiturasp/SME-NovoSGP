using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAlunosPorTurmaNaUEQueryHandler : IRequestHandler<ObterQuantidadeAlunosPorTurmaNaUEQuery, IEnumerable<AlunosPorTurmaDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterQuantidadeAlunosPorTurmaNaUEQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunosPorTurmaDto>> Handle(ObterQuantidadeAlunosPorTurmaNaUEQuery request, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient("servicoEOL");
            var url = $"escolas/{request.UeCodigo}/alunos/quantidade";

            var resposta = await httpClient.GetAsync(url);
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<AlunosPorTurmaDto>>(json);
            }
            else
            {
                string erro = $"Não foi possível obter quantidade de alunos da turma no EOL - HttpCode {(int)resposta.StatusCode} - erro: {JsonConvert.SerializeObject(resposta.RequestMessage)}";
                await mediator.Send(new SalvarLogViaRabbitCommand(erro, LogNivel.Negocio, LogContexto.Turma, string.Empty));

                throw new NegocioException(erro);
            }
        }
    }
}
