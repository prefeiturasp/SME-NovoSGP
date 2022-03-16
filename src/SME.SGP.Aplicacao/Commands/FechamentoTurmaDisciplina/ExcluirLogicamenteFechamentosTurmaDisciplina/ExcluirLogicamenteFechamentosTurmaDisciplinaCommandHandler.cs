using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirLogicamenteFechamentosTurmaDisciplinaCommandHandler : IRequestHandler<ExcluirLogicamenteFechamentosTurmaDisciplinaCommand, bool>
    {
        private readonly IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina;

        public ExcluirLogicamenteFechamentosTurmaDisciplinaCommandHandler(IRepositorioFechamentoTurmaDisciplina repositorioFechamentoTurmaDisciplina)
        {
            this.repositorioFechamentoTurmaDisciplina = repositorioFechamentoTurmaDisciplina ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaDisciplina));
        }
        public async Task<bool> Handle(ExcluirLogicamenteFechamentosTurmaDisciplinaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurmaDisciplina
                .ExcluirLogicamenteFechamentosTurmaDisciplina(request.IdsFechamentoTurmaDisciplina);
        }
    }
}
