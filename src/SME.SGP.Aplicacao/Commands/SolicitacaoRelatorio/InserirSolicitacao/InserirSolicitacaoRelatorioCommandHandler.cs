using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirSolicitacaoRelatorioCommandHandler : IRequestHandler<InserirSolicitacaoRelatorioCommand, long>
    {
        private readonly IRepositorioSolicitacaoRelatorio _repositorio;

        public InserirSolicitacaoRelatorioCommandHandler(IRepositorioSolicitacaoRelatorio repositorio)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(InserirSolicitacaoRelatorioCommand request, CancellationToken cancellationToken)
        {

            var entidade = new SolicitacaoRelatorio
            {
                ExtensaoRelatorio = request.SolicitacaoRelatorio.ExtensaoRelatorio,
                FiltrosUsados = request.SolicitacaoRelatorio.FiltrosUsados,
                Relatorio = request.SolicitacaoRelatorio.TipoRelatorio,
                UsuarioQueSolicitou = request.SolicitacaoRelatorio.UsuarioQueSolicitou,
                StatusSolicitacao = request.SolicitacaoRelatorio.StatusSolicitacao,
                Excluido = false,
                SolicitadoEm = DateTimeExtension.HorarioBrasilia()
            };

            return await _repositorio.SalvarAsync(entidade);
        }
    }
}
