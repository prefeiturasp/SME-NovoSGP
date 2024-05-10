using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSemestreAtualRelatorioQueryHandler : IRequestHandler<ObterSemestreAtualRelatorioQuery, int>
    {
        private readonly IRepositorioAcompanhamentoAlunoConsulta alunoConsulta;
        public ObterSemestreAtualRelatorioQueryHandler(IRepositorioAcompanhamentoAlunoConsulta alunoConsulta)
        {
            this.alunoConsulta = alunoConsulta ?? throw new ArgumentNullException(nameof(alunoConsulta));
        }
        public async Task<int> Handle(ObterSemestreAtualRelatorioQuery request, CancellationToken cancellationToken)
        {
            return await alunoConsulta.ObterUltimoSemestreAcompanhamentoGerado(request.AlunoCodigo);
        }
    }
}
