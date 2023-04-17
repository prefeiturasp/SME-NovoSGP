using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AlterarAulaFrequenciaTratarCommandHandler : IRequestHandler<AlterarAulaFrequenciaTratarCommand, bool>
    {
        private readonly IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno;
        private readonly IMediator mediator;

        public AlterarAulaFrequenciaTratarCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno, IMediator mediator)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(AlterarAulaFrequenciaTratarCommand request, CancellationToken cancellationToken)
        {
            var registrosFrequenciaAlunos = await mediator.Send(new ObterRegistroFrequenciaAlunoPorAulaIdQuery(request.Aula.Id), cancellationToken);

            var quantidadeAtual = request.Aula.Quantidade;
            var quantidadeOriginal = request.QuantidadeAulasOriginal;

            if (quantidadeAtual > quantidadeOriginal)
            {
                var ausenciasParaAdicionar = new List<RegistroFrequenciaAluno>();

                // Replicar o ultimo registro de frequencia
                registrosFrequenciaAlunos.Where(a => a.NumeroAula == quantidadeOriginal).ToList()
                    .ForEach(frequencia =>
                    {
                        for (var n = quantidadeOriginal + 1; n <= quantidadeAtual; n++)
                        {
                            var clone = (RegistroFrequenciaAluno)frequencia.Clone();
                            clone.NumeroAula = n;
                            ausenciasParaAdicionar.Add(clone);
                        }
                    });

                if (ausenciasParaAdicionar.Any())
                    await repositorioRegistroFrequenciaAluno.InserirVarios(ausenciasParaAdicionar);
            }
            else
            {
                // Excluir os registros de aula maior que o atual
                var idsParaExcluir = registrosFrequenciaAlunos.Where(a => a.NumeroAula > quantidadeAtual).Select(a => a.Id).ToList();

                if (idsParaExcluir.Count > 0)
                {
                    await repositorioRegistroFrequenciaAluno.RemoverLogico(idsParaExcluir.ToArray());
                    foreach (var aula in registrosFrequenciaAlunos.Where(a => a.NumeroAula > quantidadeAtual).Select(s => new { s.AulaId, s.NumeroAula }).Distinct())
                    {
                        await mediator.Send(new ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand(aula.AulaId, aula.NumeroAula), cancellationToken);
                    }
                }
            }

            return true;
        }
    }
}
