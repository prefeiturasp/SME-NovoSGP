using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesRegistroAcaoSecaoQueryHandler : IRequestHandler<ObterSecoesRegistroAcaoSecaoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoRegistroAcaoBuscaAtiva repositorioSecao;

        public ObterSecoesRegistroAcaoSecaoQueryHandler(IRepositorioSecaoRegistroAcaoBuscaAtiva repositorioSecao)
        {
            this.repositorioSecao = repositorioSecao ?? throw new System.ArgumentNullException(nameof(repositorioSecao));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesRegistroAcaoSecaoQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecao.ObterSecoesRegistroAcaoBuscaAtiva(request.RegistroAcaoId);
            return MapearParaDto(secoes);
        }

        private IEnumerable<SecaoQuestionarioDto> MapearParaDto(IEnumerable<SecaoRegistroAcaoBuscaAtiva> secoes)
        {
            foreach (var secao in secoes)
            {
                yield return new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Etapa = secao.Etapa,
                    Ordem = secao.Ordem,
                    Concluido = (secao.RegistroBuscaAtivaSecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.RegistroBuscaAtivaSecao,
                    TipoQuestionario = secao.Questionario.Tipo
                };
            }
        }
    }
}
