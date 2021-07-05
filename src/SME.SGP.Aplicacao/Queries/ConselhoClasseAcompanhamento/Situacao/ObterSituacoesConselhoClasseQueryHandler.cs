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
            foreach (var status in Enum.GetValues(typeof(StatusConselhoClasse)))
            {
                var situacao = new SituacaoDto
                {
                    Codigo = (int)status,
                    Descricao = ((StatusConselhoClasse)status).ObterAtributo<DisplayAttribute>().Description
                };
                listaSituacoes.Add(situacao);
            }
            return listaSituacoes;
        }
    }
}
