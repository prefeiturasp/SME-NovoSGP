using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.EncaminhamentoNAAPA.ObterSecaoEncaminhamentoIndividual
{

       public class ObterSecaoAtendimentoIndividualQueryHandler : IRequestHandler<ObterSecaoAtendimentoIndividualQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecaoAtendimentoIndividualQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecaoAtendimentoIndividualQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoIndividual(request.EncaminhamentoNAAPAId);

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
                    Concluido = (secao.NomeComponente == EncaminhamentoNAAPAConstants.SECAO_ITINERANCIA) || (secao.EncaminhamentoNAAPASecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoNAAPASecao,
                    TipoQuestionario = secao.Questionario.Tipo
                };
            }
        }
    }
}
