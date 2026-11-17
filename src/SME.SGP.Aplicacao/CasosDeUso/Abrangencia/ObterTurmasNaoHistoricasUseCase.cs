using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasNaoHistoricasUseCase : AbstractUseCase, IObterTurmasNaoHistoricasUseCase
    {
        private readonly IRepositorioCacheAbrangencia repositorioCacheAbrangencia;

        public ObterTurmasNaoHistoricasUseCase(IMediator mediator, IRepositorioCacheAbrangencia repositorioCacheAbrangencia) : base(mediator)
        {
            this.repositorioCacheAbrangencia = repositorioCacheAbrangencia ?? throw new ArgumentNullException(nameof(repositorioCacheAbrangencia));
        }

        public async Task<IEnumerable<TurmaNaoHistoricaDto>> Executar()
        {
            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            if (!usuario.EhAbrangenciaSomenteUE())
                return Enumerable.Empty<TurmaNaoHistoricaDto>();

            var usuarioId = usuario.Id;
            int anoLetivo = DateTime.Now.Year;

            Task<IEnumerable<TurmaNaoHistoricaDto>> Buscar()
                => mediator.Send(new ObterTurmasPorAnoEUsuarioIdQuery(usuarioId, anoLetivo));

            var configuracaoCache = await mediator.Send(ObterConfiguracaoCacheAbrangenciaQuery.Instance);

            if (configuracaoCache.EhNulo())
                return await Buscar();

            var chave = string.Format(NomeChaveCache.ABRANGENCIA_TURMAS_VIGENTES, usuarioId, usuario.PerfilAtual, anoLetivo);

            var turmas = await repositorioCacheAbrangencia.ObterComCacheAsync(chave,
                async () => (await Buscar()).Select(TurmaVigenteCache.De).ToList(),
                configuracaoCache.ObterMinutosExpiracao(usuario.PerfilAtual));

            return turmas.Select(t => t.ParaRetorno()).ToList();
        }

        public class TurmaVigenteCache
        {
            public long Id { get; set; }
            public string Codigo { get; set; }
            public int CodigoModalidade { get; set; }
            public string AnoTurma { get; set; }
            public int AnoLetivo { get; set; }
            public string Nome { get; set; }
            public string NomeFiltroOriginal { get; set; }

            public static TurmaVigenteCache De(TurmaNaoHistoricaDto turma) => new()
            {
                Id = turma.Id,
                Codigo = turma.Codigo,
                CodigoModalidade = turma.CodigoModalidade,
                AnoTurma = turma.AnoTurma,
                AnoLetivo = turma.AnoLetivo,
                Nome = turma.Nome,
                NomeFiltroOriginal = turma.ObterNomeFiltroOriginal()
            };

            public TurmaNaoHistoricaDto ParaRetorno() => new()
            {
                Id = Id,
                Codigo = Codigo,
                CodigoModalidade = CodigoModalidade,
                AnoTurma = AnoTurma,
                AnoLetivo = AnoLetivo,
                Nome = Nome,
                NomeFiltro = NomeFiltroOriginal
            };
        }
    }
}
