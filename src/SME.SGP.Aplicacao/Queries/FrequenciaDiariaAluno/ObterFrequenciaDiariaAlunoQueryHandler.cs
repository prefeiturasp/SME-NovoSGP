using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaDiariaAlunoQueryHandler : ConsultasBase, IRequestHandler<ObterFrequenciaDiariaAlunoQuery, PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>>
    {
        private IMediator mediator { get; }
        private readonly IRepositorioFrequenciaDiariaAluno repositorioFrequenciaDiaria;
        public ObterFrequenciaDiariaAlunoQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioFrequenciaDiariaAluno repositorioFrequenciaDiaria) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioFrequenciaDiaria = repositorioFrequenciaDiaria ?? throw new ArgumentNullException(nameof(repositorioFrequenciaDiaria));
        }

        public async Task<PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>> Handle(ObterFrequenciaDiariaAlunoQuery request, CancellationToken cancellationToken)
        {
            var retornoPaginado = new PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>();
            var quantidadeDiasPorTipoFrequencia = await repositorioFrequenciaDiaria.ObterQuantidadeAulasDiasTipoFrequenciaPorBimestreAlunoCodigoTurmaDisciplina(Paginacao, request.Bimestre, request.AlunoCodigo.ToString()
                , request.TurmaId, request.ComponenteCurricularId.ToString());

            var lista = MapearMotivoAusencia(quantidadeDiasPorTipoFrequencia.Items);
            retornoPaginado = new PaginacaoResultadoDto<FrequenciaDiariaAlunoDto>()
            {
                TotalPaginas = quantidadeDiasPorTipoFrequencia.TotalPaginas,
                TotalRegistros = quantidadeDiasPorTipoFrequencia.TotalRegistros,
                Items = lista.OrderByDescending(x => x.DataAula)
            };


            return retornoPaginado;
        }

        private IEnumerable<FrequenciaDiariaAlunoDto> MapearMotivoAusencia(IEnumerable<QuantidadeAulasDiasPorBimestreAlunoCodigoTurmaDisciplinaDto> quantidadeDiasPorTipoFrequencia)
        {
            var listaFrequencias = new List<FrequenciaDiariaAlunoDto>();
            foreach (var item in quantidadeDiasPorTipoFrequencia.GroupBy(x => x.DataAula))
            {
                var qtdPresenca = item.Where(x => x.AulasId == item.FirstOrDefault().AulasId && x.TipoFrequencia == (int)TipoFrequencia.C).Count();
                var totalFaltas = item.Where(x => x.AulasId == item.FirstOrDefault().AulasId && x.TipoFrequencia == (int)TipoFrequencia.F).Count();
                var totalRemoto = item.Where(x => x.AulasId == item.FirstOrDefault().AulasId && x.TipoFrequencia == (int)TipoFrequencia.R).Count();
                var totalAulasDia = item.Count();

                var frequencia = new FrequenciaDiariaAlunoDto
                {
                    Id = item.FirstOrDefault().AnotacaoId,
                    DataAula = item.FirstOrDefault().DataAula,
                    QuantidadeAulas = totalAulasDia,
                    QuantidadePresenca = qtdPresenca,
                    QuantidadeAusencia = totalFaltas > 0 ? totalFaltas :(totalAulasDia - qtdPresenca),
                    QuantidadeRemoto = totalRemoto,
                    Motivo = !string.IsNullOrEmpty(item.FirstOrDefault().MotivoAusencia) ? UtilRegex.RemoverTagsHtml(item.FirstOrDefault().MotivoAusencia) : string.Empty,
                };
                listaFrequencias.Add(frequencia);
            }
            return listaFrequencias;
        }
    }
}
