using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEEPorAlunoEAnoQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentoAEEPorAlunoEAnoQuery, EncaminhamentosAEEResumoDto>
    {
        public IMediator mediator { get; }
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }

        public ObterEncaminhamentoAEEPorAlunoEAnoQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentosAEEResumoDto>> Handle(ObterEncaminhamentosAEEQuery request, CancellationToken cancellationToken)
        {
            return await MapearParaDto(await repositorioEncaminhamentoAEE.ListarPaginado(request.DreId,
                                                                     request.UeId,
                                                                     request.TurmaId,
                                                                     request.AlunoCodigo,
                                                                     (int?)request.Situacao,
                                                                     Paginacao));
        }

        private async Task<PaginacaoResultadoDto<EncaminhamentosAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<EncaminhamentosAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<EncaminhamentosAEEResumoDto>> MapearParaDto(IEnumerable<EncaminhamentoAEEAlunoTurmaDto> encaminhamentos)
        {
            var listaEncaminhamentos = new List<EncaminhamentosAEEResumoDto>();

            foreach (var encaminhamento in encaminhamentos)
            {
                var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(encaminhamento.AlunoCodigo, encaminhamento.TurmaAno));

                listaEncaminhamentos.Add(new EncaminhamentosAEEResumoDto()
                {
                    Id = encaminhamento.Id,
                    Situacao = encaminhamento.Situacao.Name(),
                    Turma = $"{encaminhamento.TurmaModalidade.ShortName()} - {encaminhamento.TurmaNome}",
                    Numero = aluno?.NumeroAlunoChamada ?? 0,
                    Nome = aluno?.NomeAluno
                });
            }

            return listaEncaminhamentos;
        }
    }
}
