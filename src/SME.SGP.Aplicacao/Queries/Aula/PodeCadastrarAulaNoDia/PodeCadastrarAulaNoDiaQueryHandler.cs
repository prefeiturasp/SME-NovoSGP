using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaNoDiaQueryHandler : IRequestHandler<PodeCadastrarAulaNoDiaQuery, bool>
    {
        private readonly IRepositorioAula repositorioAula;
        public PodeCadastrarAulaNoDiaQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<bool> Handle(PodeCadastrarAulaNoDiaQuery request, CancellationToken cancellationToken)
            => !await repositorioAula.ExisteAulaNaDataDataTurmaDisciplinaProfessorRfAsync(request.DataAula, request.TurmaCodigo, request.ComponenteCurricular.ToString(), request.ProfessorRf);
    }
}
