using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPorIdUseCase : AbstractUseCase, IObterAulaPorIdUseCase
    {
        private readonly IConsultasDisciplina consultasDisciplina;
        
        public ObterAulaPorIdUseCase(IMediator mediator, IConsultasDisciplina consultasDisciplina) : base(mediator)
        {
            this.consultasDisciplina = consultasDisciplina ?? throw new ArgumentNullException(nameof(consultasDisciplina));
        }

        public async Task<AulaConsultaDto> Executar(long aulaId)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));
            
            if (aula == null || aula.Excluido)
                throw new NegocioException($"Aula de id {aulaId} não encontrada");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            
            var aberto = await AulaDentroDoPeriodo(aula.TurmaId, aula.DataAula);
            
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var usuarioAcessoAoComponente = await UsuarioComAcessoAoComponente(usuarioLogado, aula, usuarioLogado.EhProfessorCj());
            
            var aulaEmManutencao = await mediator.Send(new ObterAulaEmManutencaoQuery(aula.Id));

            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;
            
            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(aula.DataAula, turma));
            
            bool temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));

            return await MapearParaDto(aula, aberto, usuarioAcessoAoComponente, aulaEmManutencao, temPeriodoAberto);
        }

        private async Task<bool> AulaDentroDoPeriodo(string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(dataAula, turma));

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }

        private async Task<bool> UsuarioComAcessoAoComponente(Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            if (usuarioLogado.EhGestorEscolar())
                return true;

            var componentesUsuario = ehCJ 
                                    ? await mediator.Send(new ObterComponentesCJQuery(null, 
                                                                                      aula.TurmaId, 
                                                                                      string.Empty, 
                                                                                      long.Parse(aula.DisciplinaId), 
                                                                                      usuarioLogado.CodigoRf, false)) 
                                    : await mediator.Send(new ObterComponentesCurricularesPorTurmaLoginEPerfilQuery(aula.TurmaId, 
                                                                                                                    usuarioLogado.CodigoRf, 
                                                                                                                    usuarioLogado.PerfilAtual));

            if (aula.CriadoRF == usuarioLogado.CodigoRf)
            {
                var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));

                if (turma == null)
                    throw new NegocioException($"Turma de codigo [{aula.TurmaId}] não localizada!");

                var disciplinas = await consultasDisciplina.ObterComponentesCurricularesPorProfessorETurma(aula.TurmaId, turma.TipoTurma == TipoTurma.Programa);
                
                if (!disciplinas.Any(d => d.CodigoComponenteCurricular == long.Parse(aula.DisciplinaId)))
                    return true;
            }

            var disciplina = componentesUsuario?.FirstOrDefault(x => x.Codigo.ToString().Equals(aula.DisciplinaId));
            
            return disciplina != null;
        }

        private async Task<AulaConsultaDto> MapearParaDto(Aula aula, bool aberto, bool usuarioAcessoAoComponente, bool aulaEmManutencao, bool temPeriodoAberto)
        {
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            AulaConsultaDto dto = new AulaConsultaDto()
            {
                Id = aula.Id,
                DisciplinaId = aula.DisciplinaId,
                DisciplinaCompartilhadaId = aula.DisciplinaCompartilhadaId,
                TurmaId = aula.TurmaId,
                UeId = aula.UeId,
                TipoCalendarioId = aula.TipoCalendarioId,
                DentroPeriodo = aberto,
                TipoAula = aula.TipoAula,
                Quantidade = aula.Quantidade,
                ProfessorRf = aula.ProfessorRf,
                DataAula = aula.DataAula.Local(),
                RecorrenciaAula = aula.RecorrenciaAula,
                AlteradoEm = aula.AlteradoEm,
                AlteradoPor = aula.AlteradoPor,
                AlteradoRF = aula.AlteradoRF,
                CriadoEm = aula.CriadoEm,
                CriadoPor = aula.CriadoPor,
                CriadoRF = aula.CriadoRF,
                Migrado = aula.Migrado,
                SomenteLeitura = !usuarioAcessoAoComponente || !temPeriodoAberto,
                EmManutencao = aulaEmManutencao,
                PodeEditar = (usuarioLogado.EhProfessorCj() && aula.AulaCJ)
                          || (!aula.AulaCJ && (usuarioLogado.EhProfessor() || usuarioLogado.EhGestorEscolar()))
            };

            return dto;
        }

    }
}
