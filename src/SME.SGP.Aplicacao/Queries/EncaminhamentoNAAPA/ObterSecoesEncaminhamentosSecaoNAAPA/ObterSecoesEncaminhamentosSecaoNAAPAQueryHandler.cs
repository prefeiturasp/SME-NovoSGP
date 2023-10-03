using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesEncaminhamentosSecaoNAAPAQueryHandler : IRequestHandler<ObterSecoesEncaminhamentosSecaoNAAPAQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;
        private const string SECAO_ITINERANCIA = "QUESTOES_ITINERACIA";

        public ObterSecoesEncaminhamentosSecaoNAAPAQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesEncaminhamentosSecaoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorModalidade(request.Modalidades,request.EncaminhamentoNAAPAId);

            return MapearParaDto(secoes);
        }

        private IEnumerable<SecaoQuestionarioDto> MapearParaDto(IEnumerable<SecaoEncaminhamentoNAAPA> secoes)
        {
            foreach (var secao in secoes)
            {
                yield return new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Etapa = secao.Etapa,
                    Concluido = (secao.NomeComponente == SECAO_ITINERANCIA) || (secao.EncaminhamentoNAAPASecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoNAAPASecao,
                    TipoQuestionario = secao.Questionario.Tipo
                };
            }
        }
    }
}
