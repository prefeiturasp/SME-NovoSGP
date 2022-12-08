using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorEtapaModalidade(request.Etapas,
                request.Modalidade,request.EncaminhamentoNAAPAId);

            return MapearParaDto(secoes);
        }

        private IEnumerable<SecaoQuestionarioDto> MapearParaDto(IEnumerable<SecaoEncaminhamentoNAAPA> secoes)
        {
            foreach(var secao in secoes)
            {
                yield return new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Etapa = secao.Etapa,
                    Concluido = secao.EncaminhamentoNAAPASecao?.Concluido ?? false,
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoNAAPASecao
                };
            }
        }
    }
}
