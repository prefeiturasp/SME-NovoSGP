using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public static class ObterAulaPorIdUseCase
    {
        public static async Task<AulaConsultaDto> Executar(IMediator mediator, long aulaId)
        {
            var aula = await mediator.Send(new ObterAulaPorIdQuery(aulaId));
            if (aula == null || aula.Excluido)
                throw new NegocioException($"Aula de id {aulaId} não encontrada");

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(aula.TurmaId));
            var aberto = await AulaDentroDoPeriodo(mediator, aula.TurmaId, aula.DataAula);
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            var usuarioAcessoAoComponente = await UsuarioComAcessoAoComponente(mediator, usuarioLogado, aula, usuarioLogado.EhProfessorCj());
            var aulaEmManutencao = await mediator.Send(new ObterAulaEmManutencaoQuery(aula.Id));

            var mesmoAnoLetivo = DateTime.Today.Year == aula.DataAula.Year;
            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(aula.DataAula, turma));
            bool temPeriodoAberto = await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));

            return MapearParaDto(aula, aberto, usuarioAcessoAoComponente, aulaEmManutencao, temPeriodoAberto);
        }

        private static async Task<bool> AulaDentroDoPeriodo(IMediator mediator, string turmaCodigo, DateTime dataAula)
        {
            var turma = await mediator.Send(new ObterTurmaComUeEDrePorCodigoQuery(turmaCodigo));
            if (turma == null)
                throw new NegocioException($"Turma de codigo [{turmaCodigo}] não localizada!");

            var bimestreAula = await mediator.Send(new ObterBimestreAtualQuery(dataAula, turma));

            var mesmoAnoLetivo = DateTime.Today.Year == dataAula.Year;
            return await mediator.Send(new TurmaEmPeriodoAbertoQuery(turma, DateTime.Today, bimestreAula, mesmoAnoLetivo));
        }

        private static async Task<bool> UsuarioComAcessoAoComponente(IMediator mediator, Usuario usuarioLogado, Aula aula, bool ehCJ)
        {
            var componentesUsuario = ehCJ ?
                await mediator.Send(new ObterComponentesCJQuery(null, aula.TurmaId, string.Empty, long.Parse(aula.DisciplinaId), usuarioLogado.CodigoRf, false)) :
                await mediator.Send(new ObterComponentesCurricularesPorTurmaLoginEPerfilQuery(aula.TurmaId, usuarioLogado.CodigoRf, usuarioLogado.PerfilAtual));

            var disciplina = componentesUsuario?.FirstOrDefault(x => x.Codigo.ToString().Equals(aula.DisciplinaId));
            return disciplina != null;
        }

        private static AulaConsultaDto MapearParaDto(Aula aula, bool aberto, bool usuarioAcessoAoComponente, bool aulaEmManutencao, bool temPeriodoAberto)
        {
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
                EmManutencao = aulaEmManutencao
            };

            return dto;
        }

    }
}
