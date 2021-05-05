using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoAcompanhamentoQueryHandler : IRequestHandler<ObterTurmasFechamentoAcompanhamentoQuery, PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasFechamentoAcompanhamentoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> Handle(ObterTurmasFechamentoAcompanhamentoQuery request, CancellationToken cancellationToken)
        {
            var turmasPaginada = await repositorioTurma.ObterTurmasFechamentoAcompanhamento(request.Paginacao,
                                                                                            request.DreId,
                                                                                            request.UeId,
                                                                                            request.TurmaId,
                                                                                            request.Modalidade,
                                                                                            request.Semestre,
                                                                                            request.Bimestre,
                                                                                            request.AnoLetivo);

            return turmasPaginada;

            //var listaPaginada = new PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>()
            //{
            //    TotalPaginas = 10,
            //    TotalRegistros = 10,
            //    Items = new List<TurmaAcompanhamentoFechamentoRetornoDto>()
            //    {
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 123456,
            //            Nome = "Turma 1A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 345689,
            //            Nome = "Turma 1B",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 453567,
            //            Nome = "Turma 1C",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 3457435,
            //            Nome = "Turma 2A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 765432,
            //            Nome = "Turma 2A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 123456,
            //            Nome = "Turma 2B",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 098789,
            //            Nome = "Turma 2C",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 434564,
            //            Nome = "Turma 3A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 941267,
            //            Nome = "Turma 4A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //           TurmaId = 342367,
            //            Nome = "Turma 4B",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 675456,
            //            Nome = "Turma 5A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 543234,
            //            Nome = "Turma 5B",
            //        },
            //         new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 675456,
            //            Nome = "Turma 6A",
            //        },
            //        new TurmaAcompanhamentoFechamentoRetornoDto()
            //        {
            //            TurmaId = 543234,
            //            Nome = "Turma 6B",
            //        }
            //    }
            //};
            //return Task.FromResult(listaPaginada);
        }
    }
}
