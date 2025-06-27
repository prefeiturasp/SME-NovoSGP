using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasProfessor : AbstractUseCase, IConsultasProfessor
    {
        private readonly IRepositorioCache repositorioCache;

        public ConsultasProfessor(IRepositorioCache repositorioCache, IMediator mediator) : base(mediator)
        {
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<ProfessorTurmaDto>> Listar(string codigoRf)
        {
            return MapearParaDto(await mediator.Send(new ObterTurmasDoProfessorQuery(codigoRf)));
        }

        public async Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string ueId, string nomeProfessor)
        {
            if (string.IsNullOrEmpty(nomeProfessor) && nomeProfessor?.Length < 2)
                return null;
            var retornoProfessores = await mediator.Send(new ObterProfessoresAutoCompleteQuery(anoLetivo, dreId, ueId, nomeProfessor));
            for (int i = 0; i < retornoProfessores.Count(); i++)
            {
                var professorSgp = await ObterProfessorSGPConsultaPorNome(retornoProfessores.ToList()[i].CodigoRF);
                if (professorSgp.NaoEhNulo())
                    retornoProfessores.ToList()[i].UsuarioId = professorSgp.Id;
            }
            return retornoProfessores.Where(x => x.UsuarioId > 0);
        }

        public async Task<ProfessorResumoDto> ObterResumoPorRFUeDreAnoLetivo(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false)
        {
            var professorResumo = await ObterProfessorUeRFEOL(codigoRF, anoLetivo, dreId, ueId, buscarOutrosCargos, buscarPorTodasDre);

            return professorResumo;
        }
        
        private async Task<Usuario> ObterProfessorSGPConsultaPorNome(string codigoRF)
        {
            var usuarioSgp = await mediator.Send(new ObterUsuarioPorRfQuery(codigoRF));
            return usuarioSgp;
        }
        
        private async Task<ProfessorResumoDto> ObterProfessorUeRFEOL(string codigoRF, int anoLetivo, string dreId, string ueId, bool buscarOutrosCargos = false, bool buscarPorTodasDre = false)
        {
            var professorResumo = await mediator.Send(new ObterProfessorPorRFUeDreAnoLetivoQuery(codigoRF, anoLetivo, dreId, ueId, buscarOutrosCargos, buscarPorTodasDre));
            if (professorResumo.EhNulo())
                throw new NegocioException("RF não localizado do EOL");

            return professorResumo;
        }
        public Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo)
        {
            var chaveCache = string.Format(NomeChaveCache.TURMAS_PROFESSOR_ANO_ESCOLA, rfProfessor, anoLetivo, codigoEscola);

            return repositorioCache.ObterAsync(chaveCache,
             async () => await mediator.Send(new ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivoQuery(rfProfessor, codigoEscola, anoLetivo)),
             "Obter turmas atribuidas ao professor");
        }

        private IEnumerable<ProfessorTurmaDto> MapearParaDto(IEnumerable<ProfessorTurmaReposta> turmas)
        {
            return turmas?.Select(m => new ProfessorTurmaDto()
            {
                Ano = m.Ano,
                AnoLetivo = m.AnoLetivo,
                CodDre = m.CodDre,
                CodEscola = m.CodEscola,
                CodModalidade = m.CodModalidade,
                CodTipoEscola = m.CodTipoEscola,
                CodTipoUE = m.CodTipoUE,
                CodTurma = m.CodTurma,
                Dre = m.Dre,
                DreAbrev = m.DreAbrev,
                Modalidade = m.Modalidade,
                NomeTurma = m.NomeTurma,
                TipoEscola = m.TipoEscola,
                Semestre = m.Semestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }
    }
}