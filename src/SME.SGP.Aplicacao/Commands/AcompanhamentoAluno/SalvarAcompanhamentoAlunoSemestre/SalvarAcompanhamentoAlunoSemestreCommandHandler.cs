using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoAlunoSemestreCommandHandler : IRequestHandler<SalvarAcompanhamentoAlunoSemestreCommand, AcompanhamentoAlunoSemestre>
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorio;

        public SalvarAcompanhamentoAlunoSemestreCommandHandler(IRepositorioAcompanhamentoAlunoSemestre repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AcompanhamentoAlunoSemestre> Handle(SalvarAcompanhamentoAlunoSemestreCommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(request.Acompanhamento);

            return request.Acompanhamento;
        }
    }
}
