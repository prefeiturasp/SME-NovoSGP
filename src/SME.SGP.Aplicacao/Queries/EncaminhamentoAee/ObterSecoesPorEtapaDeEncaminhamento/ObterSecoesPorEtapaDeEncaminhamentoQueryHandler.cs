using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQueryHandler : IRequestHandler<ObterSecoesPorEtapaDeEncaminhamentoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoAEE repositorioSecaoEncaminhamentoAEE;

        public ObterSecoesPorEtapaDeEncaminhamentoQueryHandler(IRepositorioSecaoEncaminhamentoAEE repositorioSecaoEncaminhamentoAEE)
        {
            this.repositorioSecaoEncaminhamentoAEE = repositorioSecaoEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoAEE));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesPorEtapaDeEncaminhamentoQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoAEE.ObterSecoesEncaminhamentoPorEtapa(request.Etapas, request.EncaminhamentoAeeId);

            return MapearParaDto(secoes);
        }

        private IEnumerable<SecaoQuestionarioDto> MapearParaDto(IEnumerable<SecaoEncaminhamentoAEE> secoes)
        {
            foreach(var secao in secoes)
            {
                yield return new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Etapa = secao.Etapa,
                    Concluido = secao.EncaminhamentoAEESecao?.Concluido ?? false,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoAEESecao
                };
            }
        }
    }
}
