using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosPorCodigoEolNome
{
    public class ObterAlunosPorCodigoEolNomeQueryHandler : IRequestHandler<ObterAlunosPorCodigoEolNomeQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMediator mediator;

        public ObterAlunosPorCodigoEolNomeQueryHandler(IHttpClientFactory httpClientFactory, IMediator mediator)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosPorCodigoEolNomeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await ObterAlunosPorNomeCodigoEol(request.AnoLetivo, request.CodigoUe, request.CodigoTurma, request.Nome, request.CodigoEOL, request.SomenteAtivos);

                var alunoSimplesDto = new List<AlunoSimplesDto>();

                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(alunosEOL.Select(al => al.CodigoTurma.ToString()).ToArray()));

                foreach (var alunoEOL in alunosEOL.OrderBy(a => a.NomeAluno))
                {
                    var turmaAluno = turmas.NaoEhNulo() && turmas.Any() ? turmas.FirstOrDefault(t => t.CodigoTurma == alunoEOL.CodigoTurma.ToString()) : null;
                    var turmaAlunoDescricao = turmaAluno.EhNulo() ? string.Empty : $"- {turmaAluno.Nome}";

                    var alunoSimples = new AlunoSimplesDto()
                    {
                        Codigo = alunoEOL.CodigoAluno,
                        Nome = $"{alunoEOL.NomeAluno} {turmaAlunoDescricao}",
                        CodigoTurma = alunoEOL.CodigoTurma.ToString(),
                        TurmaId = turmaAluno.NaoEhNulo() ? turmaAluno.Id : 0,
                        NomeComModalidadeTurma = turmas.NaoEhNulo() && turmas.Any() ?
                        $"{alunoEOL.NomeAluno} - {OberterNomeTurmaFormatado(turmas.FirstOrDefault(t => t.CodigoTurma == alunoEOL.CodigoTurma.ToString()))}" : "",
                        Semestre = turmaAluno.NaoEhNulo() ? turmaAluno.Semestre : 0,
                        ModalidadeCodigo = turmaAluno.NaoEhNulo() ? turmaAluno.ModalidadeCodigo : 0,
                    };
                    alunoSimplesDto.Add(alunoSimples);
                }

                return alunoSimplesDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string OberterNomeTurmaFormatado(Turma turma)
        {
            var turmaNome = "";

            if (turma.NaoEhNulo())
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
        
        private async Task<IEnumerable<AlunoPorTurmaResposta>> ObterAlunosPorNomeCodigoEol(string anoLetivo, string codigoUe, long codigoTurma, string nome, long? codigoEol, bool? somenteAtivos)
        {
            var alunos = Enumerable.Empty<AlunoPorTurmaResposta>();
            var url = string.Format(ServicosEolConstants.URL_ALUNOS_UES_ANOS_LETIVOS_AUTOCOMPLETE, codigoUe, anoLetivo);

            var urlComplementar = (codigoTurma > 0 ? $"?codigoTurma={codigoTurma}" : null)
                                      + (codigoEol.HasValue ? $"{(codigoTurma > 0 ? "&" : "?") + $"codigoEol={codigoEol}"}" : "")
                                      + (nome.NaoEhNulo() ? $"{(codigoEol.NaoEhNulo() || codigoTurma > 0 ? "&" : "?") + $"nomeAluno={nome}"}" : "")
                                      + (somenteAtivos == true ? $"&somenteAtivos={somenteAtivos}" : "");

            var httpClient = httpClientFactory.CreateClient(ServicosEolConstants.SERVICO);
            
            var resposta = await httpClient.GetAsync(string.Concat(url,urlComplementar));

            if (!resposta.IsSuccessStatusCode)
                throw new NegocioException(string.Format(MensagemNegocioComuns.NAO_FOI_ENCONTRADO_ALUNOS_ATIVOS_PARA_UE ,codigoUe));

            if (resposta.StatusCode == HttpStatusCode.NoContent)
                return alunos;

            var json = await resposta.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AlunoPorTurmaResposta>>(json);
        }
    }
}
