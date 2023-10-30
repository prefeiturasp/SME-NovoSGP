using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterNotasPorBimestresUeAlunoTurmaQueryHandler : IRequestHandler<ObterNotasPorBimestresUeAlunoTurmaQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota;
        private readonly IMediator mediator;

        public ObterNotasPorBimestresUeAlunoTurmaQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorioConselhoClasseNota,
                                                              IMediator mediator)
        {
            this.repositorioConselhoClasseNota = repositorioConselhoClasseNota ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseNota));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasPorBimestresUeAlunoTurmaQuery request, CancellationToken cancellationToken)
        {
            var componentesTurma = await mediator
                .Send(new ObterDisciplinasPorCodigoTurmaQuery(request.TurmaCodigo), cancellationToken);

            var componenteConsideradoParaRegencia = componentesTurma.Any(ct => ct.Regencia) ? (await mediator
                .Send(new ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery(request.TurmaCodigo), cancellationToken)).First() : null;

            var componentesComNota = await repositorioConselhoClasseNota
                .ObterNotasBimestresAluno(request.AlunoCodigo, request.UeCodigo, request.TurmaCodigo, request.Bimestres);           

            var listaRetorno = from ct in componentesTurma
                               from b in request.Bimestres
                               join cn in componentesComNota on (ct.Regencia && componenteConsideradoParaRegencia != null ? componenteConsideradoParaRegencia.CodigoComponenteCurricular : ct.CodigoComponenteCurricular, b) equals (cn.ComponenteCurricularCodigo, cn.Bimestre) into notas
                               from nota in notas.DefaultIfEmpty()
                               where ct.RegistroFrequencia
                               orderby (b, ct.CodigoComponenteCurricular)
                               select new NotaConceitoBimestreComponenteDto()
                               {   
                                   ConselhoClasseId = nota?.ConselhoClasseId ?? 0,
                                   ConselhoClasseNotaId = nota?.ConselhoClasseNotaId ?? 0,
                                   ComponenteCurricularCodigo = ct.CodigoComponenteCurricular,
                                   ComponenteCurricularNome = ct.NomeComponenteInfantil ?? ct.Nome,
                                   ConceitoId = nota?.ConceitoId,
                                   Nota = nota?.Nota,
                                   Bimestre = b,                                  
                                   AlunoCodigo = request.AlunoCodigo,
                                   Conceito = nota?.Conceito,
                                   TurmaCodigo = request.TurmaCodigo
                               };

            return listaRetorno.Distinct();
        }
    }
}
