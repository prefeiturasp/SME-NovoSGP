using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap
{
    public class ObterIndicadoresPapSgpQueryHandler : IRequestHandler<ObterIndicadoresPapSgpQuery, IEnumerable<ContagemDificuldadePorTipoDto>>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterIndicadoresPapSgpQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<IEnumerable<ContagemDificuldadePorTipoDto>> Handle(ObterIndicadoresPapSgpQuery request, CancellationToken cancellationToken)
        {
            var indicadoresPap = new List<ContagemDificuldadePorTipoDto>();

            var dificuldadesPapColaborativo = await repositorioPapConsulta.ObterContagemDificuldadesPorTipo(TipoPap.PapColaborativo);
            if (dificuldadesPapColaborativo != null && (dificuldadesPapColaborativo.DificuldadeAprendizagem1 > 0 || 
                                                       dificuldadesPapColaborativo.DificuldadeAprendizagem2 > 0 || 
                                                       dificuldadesPapColaborativo.OutrasDificuldadesAprendizagem > 0))
            {
                indicadoresPap.Add(new ContagemDificuldadePorTipoDto
                {
                    TipoPap = TipoPap.PapColaborativo,
                    DificuldadeAprendizagem1 = dificuldadesPapColaborativo.DificuldadeAprendizagem1,
                    DificuldadeAprendizagem2 = dificuldadesPapColaborativo.DificuldadeAprendizagem2,
                    OutrasDificuldadesAprendizagem = dificuldadesPapColaborativo.OutrasDificuldadesAprendizagem
                });
            }

            var dificuldadesRecuperacao = await repositorioPapConsulta.ObterContagemDificuldadesPorTipo(TipoPap.RecuperacaoAprendizagens);
            if (dificuldadesRecuperacao != null && (dificuldadesRecuperacao.DificuldadeAprendizagem1 > 0 || 
                                                   dificuldadesRecuperacao.DificuldadeAprendizagem2 > 0 || 
                                                   dificuldadesRecuperacao.OutrasDificuldadesAprendizagem > 0))
            {
                indicadoresPap.Add(new ContagemDificuldadePorTipoDto
                {
                    TipoPap = TipoPap.RecuperacaoAprendizagens,
                    DificuldadeAprendizagem1 = dificuldadesRecuperacao.DificuldadeAprendizagem1,
                    DificuldadeAprendizagem2 = dificuldadesRecuperacao.DificuldadeAprendizagem2,
                    OutrasDificuldadesAprendizagem = dificuldadesRecuperacao.OutrasDificuldadesAprendizagem
                });
            }

            var dificuldadesPap2Ano = await repositorioPapConsulta.ObterContagemDificuldadesPorTipo(TipoPap.Pap2Ano);
            if (dificuldadesPap2Ano != null && (dificuldadesPap2Ano.DificuldadeAprendizagem1 > 0 || 
                                               dificuldadesPap2Ano.DificuldadeAprendizagem2 > 0 || 
                                               dificuldadesPap2Ano.OutrasDificuldadesAprendizagem > 0))
            {
                indicadoresPap.Add(new ContagemDificuldadePorTipoDto
                {
                    TipoPap = TipoPap.Pap2Ano,
                    DificuldadeAprendizagem1 = dificuldadesPap2Ano.DificuldadeAprendizagem1,
                    DificuldadeAprendizagem2 = dificuldadesPap2Ano.DificuldadeAprendizagem2,
                    OutrasDificuldadesAprendizagem = dificuldadesPap2Ano.OutrasDificuldadesAprendizagem
                });
            }

           return indicadoresPap;
        }
    }
}