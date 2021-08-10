using MediatR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

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
            var alunosMatriculados = new List<QuantidadeAlunoMatriculadoDTO>();
            var httpClient = httpClientFactory.CreateClient("servicoEOL");

            var parametros = "";

            if (request.AnoLetivo > 0)
                parametros += $"anoLetivo={request.AnoLetivo}";

            if (!string.IsNullOrEmpty(request.DreId) && !request.DreId.Contains("-99"))
                parametros += $"&dreCodigo={request.DreId}";

            if (!string.IsNullOrEmpty(request.UeId) && !request.UeId.Contains("-99"))
                parametros += $"&ueCodigo={request.UeId}";

            if (request.Modalidade != null && request.Modalidade.Count() > 0)
            {
                foreach (var item in request.Modalidade)
                {
                    parametros += $"&modalidade={item}";
                }
            }
                

            if (!string.IsNullOrEmpty(request.AnoTurma))
                parametros += $"&ano={request.AnoTurma}";

            if (request.Turma !=null  && !request.Turma.Contains("-99"))
            {
                foreach (var item in request.Turma)
                {
                    parametros += $"&turma={item}";
                }
            }

            if (parametros.StartsWith("&"))
                parametros = parametros.Substring(1);

            var resposta = await httpClient.GetAsync($"alunos/ano-letivo/{request.AnoLetivo}/matriculados/quantidade" + (parametros.Length > 0 ? $"?{parametros}" : ""));

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
