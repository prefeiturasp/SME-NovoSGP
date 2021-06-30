using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSituacoesConselhoClasseQueryHandler : IRequestHandler<ObterSituacoesConselhoClasseQuery, List<SituacaoDto>>
    {
        public async Task<List<SituacaoDto>> Handle(ObterSituacoesConselhoClasseQuery request, CancellationToken cancellationToken)
        {
            var listaSituacoes = new List<SituacaoDto>();

            var naoIniciado = new SituacaoDto
            {
                Codigo = (int)StatusConselhoClasse.NaoIniciado,
                Descricao = StatusConselhoClasse.NaoIniciado.ObterAtributo<DisplayAttribute>().Description
            };

            var emAndamento = new SituacaoDto
            {
                Codigo = (int)StatusConselhoClasse.EmAndamento,
                Descricao = StatusConselhoClasse.EmAndamento.ObterAtributo<DisplayAttribute>().Description
            };

            var concluido = new SituacaoDto
            {
                Codigo = (int)StatusConselhoClasse.Concluido,
                Descricao = StatusConselhoClasse.Concluido.ObterAtributo<DisplayAttribute>().Description
            };

            listaSituacoes.Add(naoIniciado);
            listaSituacoes.Add(emAndamento);
            listaSituacoes.Add(concluido);

            return listaSituacoes;

        }
    }
}
