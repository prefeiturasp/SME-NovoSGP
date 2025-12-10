using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesAtendimentosSecaoNAAPAQueryHandler : IRequestHandler<ObterSecoesAtendimentosSecaoNAAPAQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoAtendimentoNAAPA repositorioSecaoEncaminhamentoNAPPA;
        private readonly IMediator mediator;
        private readonly string COMPONENTE_ENC_INDIVIDUAL = "QUESTOES_FLUXO_ENC_INDIVIDUAL";

        public ObterSecoesAtendimentosSecaoNAAPAQueryHandler(IMediator mediator, IRepositorioSecaoAtendimentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesAtendimentosSecaoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesEncaminhamentoPorModalidade(request.Modalidade, request.EncaminhamentoNAAPAId);

            return await MapearParaDtoAsync(secoes);
        }

        private async Task<IEnumerable<SecaoQuestionarioDto>> MapearParaDtoAsync(IEnumerable<SecaoEncaminhamentoNAAPA> secoes)
        {
            var lista = new List<SecaoQuestionarioDto>();

            foreach (var secao in secoes)
            {
                lista.Add(new SecaoQuestionarioDto
                {
                    Id = secao.Id,
                    Nome = secao.NomeComponente == COMPONENTE_ENC_INDIVIDUAL ? "Encaminhamento" : secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Etapa = secao.Etapa,
                    Concluido = (secao.NomeComponente == AtendimentoNAAPAConstants.SECAO_ITINERANCIA) || (secao.EncaminhamentoNAAPASecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.EncaminhamentoNAAPASecao,
                    TipoQuestionario = secao.Questionario.Tipo,
                    EncaminhamentoEscolarId = secao.EncaminhamentoNAAPASecao?.EncaminhamentoEscolarId,
                    EncaminhamentoEscolar = await ObterEncamaminhamentoEscolar(secao)
                });
            }

            return lista;
        }

        private async Task<IEnumerable<QuestaoDto>> ObterEncamaminhamentoEscolar(SecaoEncaminhamentoNAAPA secao)
        {
            if (secao.EncaminhamentoNAAPASecao?.EncaminhamentoEscolar?.NaoEhNulo() == true)
            {
                return await mediator.Send(
                    new ObterQuestionarioNovoEncaminhamentoNAAPAQuery(
                        secao.QuestionarioId,
                        secao.EncaminhamentoNAAPASecao.EncaminhamentoEscolar.Id,
                        secao.EncaminhamentoNAAPASecao.EncaminhamentoEscolar.AlunoCodigo,
                        secao.EncaminhamentoNAAPASecao.EncaminhamentoEscolar.TurmaId.ToString()));
            }

            return null;
        }
    }
}
