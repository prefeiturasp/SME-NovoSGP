using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioDiarioBordo repositorio;

        public ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQueryHandler(IRepositorioDiarioBordo repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
            var retornoConsulta =  await repositorio.ObterQuantidadeTotalDeDiariosPendentesPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade);
            return MontarDto(retornoConsulta);
        }

        private IEnumerable<GraficoBaseDto> MontarDto(IEnumerable<QuantidadeTotalDiariosPendentesPorAnoETurmaDTO> retornoConsulta)
        {
            var listaGraficos = new List<GraficoBaseDto>();
            foreach (var item in retornoConsulta)
            {
                listaGraficos.Add(new GraficoBaseDto() { Descricao = item.Ano == 0 ? item.Turma : item.Ano.ToString() , Quantidade = item.QuantidadeTotalDiariosPendentes });
            }
            return listaGraficos;
        }
    }
}
