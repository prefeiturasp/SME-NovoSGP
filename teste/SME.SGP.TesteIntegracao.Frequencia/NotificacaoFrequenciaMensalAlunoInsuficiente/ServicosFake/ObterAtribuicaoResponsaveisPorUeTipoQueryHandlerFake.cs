using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.NotificacaoFrequenciaMensalAlunoInsuficiente.ServicosFake
{
    public class ObterAtribuicaoResponsaveisPorUeTipoQueryHandlerFake : IRequestHandler<ObterAtribuicaoResponsaveisPorUeTipoQuery, IEnumerable<AtribuicaoResponsavelDto>>
    {

        public async Task<IEnumerable<AtribuicaoResponsavelDto>> Handle(ObterAtribuicaoResponsaveisPorUeTipoQuery request, CancellationToken cancellationToken)
        {
            var funcionariosUnidade = new List<AtribuicaoResponsavelDto>()
            {
                new AtribuicaoResponsavelDto()
                {
                    CodigoRF = "0000002",
                    NomeResponsavel = "Psicólogo Escolar",
                },
                new AtribuicaoResponsavelDto()
                {
                    CodigoRF = "0000003",
                    NomeResponsavel = "Psicopedagogo"
                },
                new AtribuicaoResponsavelDto()
                {
                    CodigoRF = "0000004",
                    NomeResponsavel = "Assistente Social"
                }
            };


            return funcionariosUnidade.Where(f => f.NomeResponsavel.Equals(request.Tipo.ObterNome()));
        }
    }
}
