using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesAtendimentosSecaoNAAPAQueryHandler : IRequestHandler<ObterSecoesAtendimentosSecaoNAAPAQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecoesAtendimentosSecaoNAAPAQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesAtendimentosSecaoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorModalidade(request.Modalidade,request.EncaminhamentoNAAPAId);

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
                    Concluido = (secao.NomeComponente == AtendimentoNAAPAConstants.SECAO_ITINERANCIA) || (secao.EncaminhamentoNAAPASecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoNAAPASecao,
                    TipoQuestionario = secao.Questionario.Tipo
                };
            }
        }
    }
}
