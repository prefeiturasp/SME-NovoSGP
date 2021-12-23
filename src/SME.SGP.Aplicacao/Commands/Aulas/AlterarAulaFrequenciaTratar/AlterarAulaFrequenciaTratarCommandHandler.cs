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
        private readonly IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno;
        private readonly IMediator mediator;

        public AlterarAulaFrequenciaTratarCommandHandler(IRepositorioRegistroAusenciaAluno repositorioRegistroAusenciaAluno, IMediator mediator)
        {
            this.repositorioRegistroAusenciaAluno = repositorioRegistroAusenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroAusenciaAluno));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(AlterarAulaFrequenciaTratarCommand request, CancellationToken cancellationToken)
        {
            //Obter as ausencias pela aula id
            var ausencias = await mediator.Send(new ObterRegistrosAusenciaPorAulaQuery(request.Aula.Id));

            var quantidadeAtual = request.Aula.Quantidade;
            var quantidadeOriginal = request.QuantidadeAulasOriginal;

            if (quantidadeAtual > quantidadeOriginal)
            {
                var ausenciasParaAdicionar = new List<RegistroAusenciaAluno>();

                // Replicar o ultimo registro de frequencia
                ausencias.Where(a => a.NumeroAula == quantidadeOriginal).ToList()
                    .ForEach(ausencia =>
                    {
                        for (var n = quantidadeOriginal + 1; n <= quantidadeAtual; n++)
                        {
                            var clone = (RegistroAusenciaAluno)ausencia.Clone();
                            clone.NumeroAula = n;
                            ausenciasParaAdicionar.Add(clone);
                        }
                    });

                if (ausenciasParaAdicionar.Any())
                    await repositorioRegistroAusenciaAluno.SalvarVarios(ausenciasParaAdicionar);

            }
            else
            {
                // Excluir os registros de aula maior que o atual
                var idsParaExcluir = ausencias.Where(a => a.NumeroAula > quantidadeAtual).Select(a => a.Id).ToList();

                //TODO: Criar método genérico com Auditoria
                if (idsParaExcluir.Count > 0)
                    await repositorioRegistroAusenciaAluno.ExcluirVarios(idsParaExcluir);                

            }

            return true;
        }
    }
}
