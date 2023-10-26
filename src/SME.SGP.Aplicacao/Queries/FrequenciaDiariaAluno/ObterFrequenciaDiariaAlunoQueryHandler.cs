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
            int[] bimestres = request.Bimestre > 0
                ? new int[] { request.Bimestre }
                : request.Semestre == 1 ? new int[] { 1, 2 } : new int[] { 3, 4 };

            var turma = await mediator.Send(new ObterTurmaPorIdQuery(request.TurmaId));
            var codigosComponentesConsiderados = new List<long>() { request.ComponenteCurricularId };
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var quantidadeDiasPorTipoFrequencia = await repositorioFrequenciaDiaria.ObterQuantidadeAulasDiasTipoFrequenciaPorBimestreAlunoCodigoTurmaDisciplina(Paginacao, bimestres, request.AlunoCodigo.ToString()
                , request.TurmaId, codigosComponentesConsiderados.Select(c => c.ToString()).ToArray());

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

            foreach (var item in quantidadeDiasPorTipoFrequencia)
            {

                var frequencia = new FrequenciaDiariaAlunoDto
                {
                    Id = item.AnotacaoId,
                    DataAula = item.DataAula,
                    QuantidadeAulas = item.TotalAulasNoDia,
                    QuantidadePresenca = item.TotalPresencas,
                    QuantidadeAusencia = item.TotalAusencias,
                    QuantidadeRemoto = item.TotalRemotos,
                    Motivo = !string.IsNullOrEmpty(item.MotivoAusencia) ? UtilRegex.RemoverTagsHtml(item.MotivoAusencia) : string.Empty,
                };
                listaFrequencias.Add(frequencia);
            }
            return listaFrequencias;
        }
    }
}
