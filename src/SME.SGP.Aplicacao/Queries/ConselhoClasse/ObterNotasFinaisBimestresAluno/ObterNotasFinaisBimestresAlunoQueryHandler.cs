using MediatR;
using SME.SGP.Infra;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public  class ObterNotasFinaisBimestresAlunoQueryHandler : IRequestHandler<ObterNotasFinaisBimestresAlunoQuery, IEnumerable<NotaConceitoBimestreComponenteDto>>
    {
        private readonly IRepositorioConselhoClasseNotaConsulta repositorio;
        private readonly IRepositorioCache cache;

        public ObterNotasFinaisBimestresAlunoQueryHandler(IRepositorioConselhoClasseNotaConsulta repositorio, IRepositorioCache cache)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task<IEnumerable<NotaConceitoBimestreComponenteDto>> Handle(ObterNotasFinaisBimestresAlunoQuery request, CancellationToken cancellationToken)
        {
            var notasConceitosFechamento = new List<NotaConceitoBimestreComponenteDto>();
            var notasConceitosConselhoClasse = new List<NotaConceitoBimestreComponenteDto>();
            foreach (var turmaCodigo in request.TurmasCodigos)
            {
                cancellationToken.ThrowIfCancellationRequested();
                // Compartilha a consulta apenas nesta turma/requisição, sem criar nova chave de cache.
                Task<NotasFechamentoConselhoDto> fontes = null;
                async Task<NotasFechamentoConselhoDto> ObterFontes()
                {
                    try
                    {
                        fontes ??= repositorio.ObterFontesNotasFechamentoConselhoAsync(
                            turmaCodigo, request.Bimestre, request.TipoCalendario, request.AlunoCodigo);
                        return await fontes;
                    }
                    catch
                    {
                        // Permite a nova tentativa já prevista pelo cache, sem reter a tarefa com falha.
                        fontes = null;
                        throw;
                    }
                }
                var chaveConselho = string.Format(NomeChaveCache.NOTA_CONCEITO_CONSELHO_CLASSE_TURMA_BIMESTRE_ALUNO,
                    turmaCodigo, request.Bimestre, request.AlunoCodigo);
                IEnumerable<NotaConceitoBimestreComponenteDto> conselho = null;
                var fechamento = await cache.ObterAsync(
                    string.Format(NomeChaveCache.NOTA_CONCEITO_FECHAMENTO_TURMA_ALUNO_BIMESTRES_E_FINAL, turmaCodigo, request.AlunoCodigo),
                    async () =>
                    {
                        conselho = await cache.ObterAsync(chaveConselho,
                            async () => (await ObterFontes()).Conselho, "Obter notas ou conceitos do conselho de classe");
                        // Se apenas fechamento falta, não consulta novamente a fonte conselho já cacheada.
                        return fontes != null ? (await ObterFontes()).Fechamento
                            : await repositorio.ObterNotasConceitosFechamentoPorTurmaCodigoEBimestreAsync(
                                turmaCodigo, request.Bimestre, tipoCalendario: request.TipoCalendario, alunoCodigo: request.AlunoCodigo);
                    }, "Obter notas ou conceitos do fechamento.");
                if (conselho == null)
                    conselho = await cache.ObterAsync(chaveConselho,
                        () => repositorio.ObterNotasConceitosConselhoClassePorTurmaCodigoEBimestreAsync(
                            turmaCodigo, request.Bimestre == 0 ? (int?)null : request.Bimestre,
                            tipoCalendario: request.TipoCalendario, alunoCodigo: request.AlunoCodigo),
                        "Obter notas ou conceitos do conselho de classe");
                notasConceitosFechamento.AddRange(fechamento);
                notasConceitosConselhoClasse.AddRange(conselho.Where(c => c.NotaConceito.HasValue));
            }
                 
            var notasFinais = new List<NotaConceitoBimestreComponenteDto>();
            
            notasFinais.AddRange(notasConceitosConselhoClasse);
            notasFinais.AddRange(notasConceitosFechamento.Where(fechamento => 
                !notasConceitosConselhoClasse.Any(conselho => conselho.ComponenteCurricularCodigo == fechamento.ComponenteCurricularCodigo && conselho.Bimestre == fechamento.Bimestre)));
            return notasFinais;
        }
    }
}
