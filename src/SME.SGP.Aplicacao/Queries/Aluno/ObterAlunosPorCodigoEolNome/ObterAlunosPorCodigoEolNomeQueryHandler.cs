using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Aluno.ObterAlunosPorCodigoEolNome
{
    public class ObterAlunosPorCodigoEolNomeQueryHandler : IRequestHandler<ObterAlunosPorCodigoEolNomeQuery, IEnumerable<AlunoSimplesDto>>
    {
        private readonly IServicoEol servicoEOL;
        private readonly IMediator mediator;

        public ObterAlunosPorCodigoEolNomeQueryHandler(IServicoEol servicoEOL, IMediator mediator)
        {
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AlunoSimplesDto>> Handle(ObterAlunosPorCodigoEolNomeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var alunosEOL = await servicoEOL.ObterAlunosPorNomeCodigoEol(request.AnoLetivo, request.CodigoUe, request.CodigoTurma, request.Nome, request.CodigoEOL);

                var alunoSimplesDto = new List<AlunoSimplesDto>();

                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(alunosEOL.Select(al => al.CodigoTurma.ToString()).ToArray()));

                foreach (var alunoEOL in alunosEOL.OrderBy(a => a.NomeAluno))
                {
                    var turmaAluno = turmas != null && turmas.Any() ? turmas.FirstOrDefault(t => t.CodigoTurma == alunoEOL.CodigoTurma.ToString()) : null;
                    var alunoSimples = new AlunoSimplesDto()
                    {
                        Codigo = alunoEOL.CodigoAluno,
                        Nome = alunoEOL.NomeAluno,
                        CodigoTurma = alunoEOL.CodigoTurma.ToString(),
                        TurmaId = turmaAluno!=null ? turmaAluno.Id : 0,
                        NomeComModalidadeTurma = turmas != null && turmas.Any() ?
                        $"{alunoEOL.NomeAluno} - {OberterNomeTurmaFormatado(turmas.FirstOrDefault(t => t.CodigoTurma == alunoEOL.CodigoTurma.ToString()))}" : ""
                    };
                    alunoSimplesDto.Add(alunoSimples);
                }

                return alunoSimplesDto;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string OberterNomeTurmaFormatado(Turma turma)
        {
            var turmaNome = "";

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }
    }
}
