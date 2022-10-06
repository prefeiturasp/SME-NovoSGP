using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEQueryHandler : ConsultasBase, IRequestHandler<ObterEncaminhamentosAEEQuery, PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>>
    {
        public IMediator mediator { get; }
        private readonly IConsultasAbrangencia consultasAbrangencia;
        public IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE { get; }


        public ObterEncaminhamentosAEEQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioEncaminhamentoAEE repositorioEncaminhamentoAEE, IConsultasAbrangencia consultasAbrangencia) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEE = repositorioEncaminhamentoAEE ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEE));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> Handle(ObterEncaminhamentosAEEQuery request, CancellationToken cancellationToken)
        {

            int periodo = 0;
            int[] tipos = new int[0];
            List<string> turmasCodigos = new List<string>();

            var ueCodigo = mediator.Send(new ObterUePorIdQuery(request.UeId)).Result;

            var turmas = await mediator.Send(new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(ueCodigo.CodigoUe, 0, periodo, false, DateTime.Now.Year, tipos, true));

            if (turmas != null || turmas.Any())
            {
                foreach (var item in turmas)
                    turmasCodigos.Add(item.Codigo);
            }

            return await MapearParaDto(await repositorioEncaminhamentoAEE.ListarPaginado(request.DreId,
                                                                     request.UeId,
                                                                     request.TurmaId,
                                                                     request.AlunoCodigo,
                                                                     (int?)request.Situacao,
                                                                     request.ResponsavelRf,
                                                                     request.AnoLetivo,
                                                                     turmasCodigos.ToArray(),
                                                                     Paginacao));
        }

        private async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> MapearParaDto(PaginacaoResultadoDto<EncaminhamentoAEEAlunoTurmaDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<EncaminhamentoAEEResumoDto>> MapearParaDto(IEnumerable<EncaminhamentoAEEAlunoTurmaDto> encaminhamentos)
        {
            var listaEncaminhamentos = new List<EncaminhamentoAEEResumoDto>();

            foreach (var encaminhamento in encaminhamentos)
            {
                var retorno = await mediator.Send(new ObterTurmasAlunoPorFiltroQuery(encaminhamento.AlunoCodigo, encaminhamento.TurmaAno, false));
                var aluno = retorno.OrderByDescending(a => a.DataSituacao)?.FirstOrDefault();                

                var ehAtendidoAEE = await mediator.Send(new VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery(aluno.CodigoAluno, encaminhamento.TurmaAno));
                listaEncaminhamentos.Add(new EncaminhamentoAEEResumoDto()
                {
                    Id = encaminhamento.Id,
                    Situacao = encaminhamento.Situacao != 0 ? encaminhamento.Situacao.Name() : "",
                    Turma = $"{encaminhamento.TurmaModalidade.ShortName()} - {encaminhamento.TurmaNome}",
                    Numero = aluno?.NumeroAlunoChamada ?? 0,
                    Nome = aluno?.NomeAluno,
                    Responsavel = encaminhamento.Responsavel,
                    EhAtendidoAEE = ehAtendidoAEE
                });
            }

            return listaEncaminhamentos;
        }
    }
}
