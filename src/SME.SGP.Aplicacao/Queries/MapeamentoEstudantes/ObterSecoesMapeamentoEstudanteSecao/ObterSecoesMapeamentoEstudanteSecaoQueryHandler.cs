﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesMapeamentoEstudanteSecaoQueryHandler : IRequestHandler<ObterSecoesMapeamentoEstudanteSecaoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoMapeamentoEstudante repositorioSecao;

        public ObterSecoesMapeamentoEstudanteSecaoQueryHandler(IRepositorioSecaoMapeamentoEstudante repositorioSecao)
        {
            this.repositorioSecao = repositorioSecao ?? throw new System.ArgumentNullException(nameof(repositorioSecao));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesMapeamentoEstudanteSecaoQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecao.ObterSecoesMapeamentoEstudante(request.MapeamentoEstudanteId);
            return MapearParaDto(secoes);
        }

        private IEnumerable<SecaoQuestionarioDto> MapearParaDto(IEnumerable<SecaoMapeamentoEstudante> secoes)
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
                    Concluido = (secao.MapeamentoEstudanteSecao?.Concluido ?? false),
                    NomeComponente = secao.NomeComponente,
                    Auditoria = (AuditoriaDto)secao.MapeamentoEstudanteSecao,
                    TipoQuestionario = secao.Questionario.Tipo
                };
            }
        }
    }
}
