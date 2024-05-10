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
    public class ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQueryHandler : IRequestHandler<ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery, IEnumerable<GraficoBaseDto>>
    {
        private readonly IRepositorioRegistroIndividual repositorio;

        public ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQueryHandler(IRepositorioRegistroIndividual repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Handle(ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery request, CancellationToken cancellationToken)
        {
            var retornoConsulta = await repositorio.ObterQuantidadeRegistrosIndividuaisPorAnoTurmaAsync(request.AnoLetivo, request.DreId, request.UeId, request.Modalidade);

            if(retornoConsulta.NaoEhNulo() && retornoConsulta.Any())
                return MontarDto(retornoConsulta, request);

            return default;
        }

        private IEnumerable<GraficoBaseDto> MontarDto(IEnumerable<QuantidadeRegistrosIndividuaisPorAnoTurmaDTO> retornoConsulta, ObterQuantidadeRegistrosIndividuaisPorAnoTurmaQuery request)
        {            
            foreach (var item in retornoConsulta)
            {
                yield return new GraficoBaseDto()
                {
                    Descricao =  item.ObterDescricaoTurmaAno(request.UeId > 0, string.IsNullOrEmpty(item.Ano) ? item.Turma : item.Ano.ToString(), request.Modalidade),
                    Quantidade = item.QuantidadeRegistrosIndividuais
                };
            }            
        }       
    }
}
