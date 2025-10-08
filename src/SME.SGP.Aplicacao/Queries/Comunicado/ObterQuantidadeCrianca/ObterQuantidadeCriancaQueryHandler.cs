using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeCriancaQueryHandler : IRequestHandler<ObterQuantidadeCriancaQuery, QuantidadeCriancaDto>
    {
        private readonly IHttpClientFactory httpClientFactory;
        public ObterQuantidadeCriancaQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<QuantidadeCriancaDto> Handle(ObterQuantidadeCriancaQuery request, CancellationToken cancellationToken)
        {
            var quantidadeCrianca = new QuantidadeCriancaDto();
            var alunosMatriculados = Enumerable.Empty<QuantidadeAlunoMatriculadoDTO>();
            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);

            var parametros = "";

            if (request.AnoLetivo > 0)
                parametros += $"AnoLetivo={request.AnoLetivo}";

            if (!string.IsNullOrEmpty(request.DreId) && !request.DreId.Contains("-99"))
                parametros += $"&dreCodigo={request.DreId}";

            if (!string.IsNullOrEmpty(request.UeId) && !request.UeId.Contains("-99"))
                parametros += $"&ueCodigo={request.UeId}";

            if (request.Modalidade.NaoEhNulo() && request.Modalidade.Count() > 0)
            {
                foreach (var item in request.Modalidade)
                {
                    parametros += $"&modalidade={item}";
                }
            }

            if (request.AnoTurma.NaoEhNulo() && request.AnoTurma.Count() > 0)
            {
                foreach (var item in request.AnoTurma)
                {
                    parametros += $"&ano={item}";
                }
            }

            if (request.Turma.NaoEhNulo() && !request.Turma.Contains("-99"))
            {
                foreach (var item in request.Turma)
                {
                    parametros += $"&turma={item}";
                }
            }

            if (parametros.StartsWith("&"))
                parametros = parametros.Substring(1);

            var resposta = await httpClient.GetAsync(string.Format(ServicosEolConstants.URL_ALUNOS_ANO_LETIVO_MATRICULADOS_QUANTIDADE, request.AnoLetivo) + (parametros.Length > 0 ? $"?{parametros}" : ""));

            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                alunosMatriculados = JsonConvert.DeserializeObject<List<QuantidadeAlunoMatriculadoDTO>>(json);
                if (alunosMatriculados.Any())
                    quantidadeCrianca.MensagemQuantidade = $"Os Responsáveis de {alunosMatriculados.Sum(x => x.Quantidade)} crianças/estudantes poderão receber este comunicado";
            }
            else
                quantidadeCrianca.MensagemQuantidade = null;

            return quantidadeCrianca;
        }
    }
}
