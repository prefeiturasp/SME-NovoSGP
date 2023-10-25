using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQueryHandler : IRequestHandler<ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQuery, IEnumerable<OpcaoDropdownDto>>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<OpcaoDropdownDto>> Handle(ObterFiltroRelatoriosAnosEscolaresPorModalidadeUeQuery request, CancellationToken cancellationToken)
        {
            if (request.CodigoUe == "-99")
                return ObterAnosEscolaresPorModalidade(request.Modalidade);

            var modalidadesUe = await repositorioAbrangencia.ObterModalidadesPorUe(request.CodigoUe);
            var modalidadeSelecionada = modalidadesUe?.FirstOrDefault(c => c == request.Modalidade);
            if (modalidadeSelecionada.EhNulo())
                throw new NegocioException("Modalidade localizada na UE informada.");

            return ObterAnosEscolaresPorModalidade(modalidadeSelecionada.Value);
        }

        private IEnumerable<OpcaoDropdownDto> ObterAnosEscolaresPorModalidade(Modalidade modalidade)
        {
            var lista = new List<OpcaoDropdownDto>()
            {
                new OpcaoDropdownDto("-99", "Todos")
            };
            switch (modalidade)
            {
                case Modalidade.Fundamental:
                    lista.AddRange(Enumerable.Range(1, 9).Select(c => new OpcaoDropdownDto(c.ToString(), $"{c}º ano - {modalidade.Name()}")));
                    break;
                case Modalidade.Medio:
                    lista.AddRange(Enumerable.Range(1, 3).Select(c => new OpcaoDropdownDto(c.ToString(), $"{c}º ano - {modalidade.Name()}")));
                    break;
                case Modalidade.EJA:
                case Modalidade.EducacaoInfantil:
                    return lista;
                default:
                    throw new NegocioException("Modalidade não atendida pelo SGP.");
            }
            return lista;
        }
    }
}
