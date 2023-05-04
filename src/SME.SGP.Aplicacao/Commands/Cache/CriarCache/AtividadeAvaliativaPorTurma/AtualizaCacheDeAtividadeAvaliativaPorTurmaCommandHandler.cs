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
    public class AtualizaCacheDeAtividadeAvaliativaPorTurmaCommandHandler : IRequestHandler<AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand, IEnumerable<NotaConceito>>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioNotasConceitosConsulta repositorioNotasConceitos;

        public AtualizaCacheDeAtividadeAvaliativaPorTurmaCommandHandler(IRepositorioCache repositorioCache, IRepositorioNotasConceitosConsulta repositorioNotasConceitos)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioNotasConceitos = repositorioNotasConceitos ?? throw new ArgumentNullException(nameof(repositorioNotasConceitos));
        }

        public async Task<IEnumerable<NotaConceito>> Handle(AtualizaCacheDeAtividadeAvaliativaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            var nomeChave = $"Atividade-Avaliativa-{request.CodigoTurma}";

            var atividadeAvaliativas = await repositorioCache.ObterObjetoAsync<List<NotaConceito>>(nomeChave);

            if (atividadeAvaliativas == null)
                return null;

            if (request.EntidadesExcluir.Count() > 0)
                foreach (var excluir in request.EntidadesExcluir)
                    atividadeAvaliativas.Remove(excluir);

            foreach (var inserir in request.EntidadesSalvar)
            {
                atividadeAvaliativas.Add(inserir);
            }

            foreach (var alterar in request.EntidadesAlterar)
            {
                var atividade = atividadeAvaliativas.Find(atividade => atividade.Id == alterar.Id);

                    if (atividade != null)
                    {
                        atividade.Nota = alterar.Nota;
                        atividade.ConceitoId = alterar.ConceitoId;
                        atividade.AlteradoEm = alterar.AlteradoEm;
                        atividade.AlteradoPor = alterar.AlteradoPor;
                    }
                }

            await repositorioCache.SalvarAsync(nomeChave, atividadeAvaliativas);

            return atividadeAvaliativas;
        }
    }
}
