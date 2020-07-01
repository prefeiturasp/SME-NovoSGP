using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aulas
{
    public class TestePostgreCommandHandler : IRequestHandler<TestePostgreCommand, bool>
    {
        private readonly IRepositorioTestePostgre repositorioTestePostgre;

        public TestePostgreCommandHandler(IRepositorioTestePostgre repositorioTestePostgre)
        {
            this.repositorioTestePostgre = repositorioTestePostgre;
        }
        public async Task<bool> Handle(TestePostgreCommand request, CancellationToken cancellationToken)
        {

            repositorioTestePostgre.Salvar(new Ciclo()
            {
                Descricao = $"teste{DateTime.Now.Millisecond}"
            });

            return await Task.FromResult(true);
        }
    }
}
