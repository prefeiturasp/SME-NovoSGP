using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryFake : IRequestHandler<ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery, InformacoesAtualizadasMapeamentoEstudanteAlunoDto>
    {

        public ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQueryFake() 
        {}

        public Task<InformacoesAtualizadasMapeamentoEstudanteAlunoDto> Handle(ObterInformacoesAtualizadasAlunoMapeamentoEstudanteQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new InformacoesAtualizadasMapeamentoEstudanteAlunoDto()
        {
            TurmaAnoAnterior = "EF-5B",
            AnotacoesPedagogicasBimestreAnterior = "ANOTAÇÕES PEDAGÓGICAS DO BIMESTRE ANTERIOR",
            DescricaoParecerConclusivoAnoAnterior = "Promovido",
            IdParecerConclusivoAnoAnterior = 2,
            QdadeBuscasAtivasBimestre = 5,
            QdadeEncaminhamentosNAAPAAtivos = 3,
            QdadePlanosAEEAtivos = 2
        });
    }
}
