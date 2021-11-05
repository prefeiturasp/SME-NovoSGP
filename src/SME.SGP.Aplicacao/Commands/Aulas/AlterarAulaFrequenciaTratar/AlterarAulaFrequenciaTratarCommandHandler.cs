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

        public AlterarAulaFrequenciaTratarCommandHandler(IRepositorioRegistroFrequenciaAluno repositorioRegistroFrequenciaAluno)
        {
            this.repositorioRegistroFrequenciaAluno = repositorioRegistroFrequenciaAluno ?? throw new ArgumentNullException(nameof(repositorioRegistroFrequenciaAluno));
        }
        public async Task<bool> Handle(AlterarAulaFrequenciaTratarCommand request, CancellationToken cancellationToken)
        {
            //Obter as ausencias pela aula id
            var ausencias = await repositorioRegistroFrequenciaAluno.ObterRegistrosAusenciaPorAulaAsync(request.Aula.Id);
            var quantidadeAtual = request.Aula.Quantidade;
            var quantidadeOriginal = request.QuantidadeAulasOriginal;

            if (quantidadeAtual > quantidadeOriginal)
            {
                var ausenciasParaAdicionar = new List<RegistroFrequenciaAluno>();

                // Replicar o ultimo registro de frequencia
                ausencias.Where(a => a.NumeroAula == quantidadeOriginal).ToList()
                    .ForEach(ausencia =>
                    {
                        for (var n = quantidadeOriginal + 1; n <= quantidadeAtual; n++)
                        {
                            var clone = (RegistroFrequenciaAluno)ausencia.Clone();
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
                var idsParaExcluir = ausencias.Where(a => a.NumeroAula > quantidadeAtual).Select(a => a.Id).ToList();

                //TODO: Criar método genérico com Auditoria
                if (idsParaExcluir.Count > 0)
                    await repositorioRegistroFrequenciaAluno.ExcluirVarios(idsParaExcluir);                

            }

            return true;
        }
    }
}
