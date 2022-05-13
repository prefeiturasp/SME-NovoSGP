using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasPorUsuarioQueryHandler : ConsultasBase, IRequestHandler<ObterPendenciasPorUsuarioQuery, PaginacaoResultadoDto<PendenciaDto>>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IMediator mediator;

        public ObterPendenciasPorUsuarioQueryHandler(IContextoAplicacao contextoAplicacao, IMediator mediator, IRepositorioPendencia repositorioPendencia) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
        }

        public async Task<PaginacaoResultadoDto<PendenciaDto>> Handle(ObterPendenciasPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var pendencias = new PaginacaoResultadoDto<Pendencia>();

            if (!string.IsNullOrEmpty(request.TurmaCodigo) || !string.IsNullOrEmpty(request.TituloPendencia) || request.TipoPendencia > 0)
            {
                int[] tiposPendenciasAFiltrar = request.TipoPendencia > 0 ? RetornaTiposPendenciaGrupo((TipoPendenciaGrupo)request.TipoPendencia).ToArray() : new int[] { };
                pendencias = await repositorioPendencia.ListarPendenciasUsuarioComFiltro(request.UsuarioId,
                                                                                             tiposPendenciasAFiltrar.ToArray(),
                                                                                             request.TituloPendencia,
                                                                                             request.TurmaCodigo,
                                                                                             Paginacao,
                                                                                             request.TipoPendencia);

                var itensDaLista = pendencias.Items.ToList();

                if (!string.IsNullOrEmpty(request.TurmaCodigo) && request.TipoPendencia == 0)
                {
                    foreach (var pendencia in pendencias.Items)
                    {
                        var pendenciaFiltrada = await repositorioPendencia
                                                       .FiltrarListaPendenciasUsuario(request.TurmaCodigo,
                                                                                      pendencia);

                        if (pendenciaFiltrada == null)
                            itensDaLista.Remove(pendencia);
                    }
                }

                pendencias.Items = itensDaLista;
            }
            else
            {
                pendencias = await repositorioPendencia.ListarPendenciasUsuarioSemFiltro(request.UsuarioId,
                                                                                             Paginacao);
            }        

            return await MapearParaDtoPaginado(pendencias);
        }

        public IEnumerable<int> RetornaTiposPendenciaGrupo(TipoPendenciaGrupo tipoGrupo)
        {
            var tiposPendencias = Enum.GetValues(typeof(TipoPendencia))
                           .Cast<TipoPendencia>()
                           .Select(d => new { codigo = (int)d, descricao = d.ObterNomeGrupo() })
                           .Where(d => d.descricao == tipoGrupo.Name())
                           .ToList();
            return tiposPendencias.Select(tp => tp.codigo);
        }

        private async Task<PaginacaoResultadoDto<PendenciaDto>> MapearParaDtoPaginado(PaginacaoResultadoDto<Pendencia> pendenciasPaginadas)
        {
            return new PaginacaoResultadoDto<PendenciaDto>()
            {
                Items = await MapearParaDto(pendenciasPaginadas.Items),
                TotalPaginas = pendenciasPaginadas.TotalPaginas,
                TotalRegistros = pendenciasPaginadas.TotalRegistros
            };
        }

        private async Task<IEnumerable<PendenciaDto>> MapearParaDto(IEnumerable<Pendencia> pendencias)
        {
            var listaPendenciasDto = new List<PendenciaDto>();
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            foreach (var pendencia in pendencias)
            {
                //var pendenciasDiarios = await mediator.Send(new ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(pendencia.Id, codigoRfProfessor));
                //xcvxcvxcvcxvcx
                //listaPendenciasDto.Add(new PendenciaDto()
                //{
                //    Tipo = pendencia.Tipo.GroupName(),
                //    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                //    Detalhe = await ObterDescricaoPendencia(pendencia, usuarioLogado.CodigoRf),
                //    Turma = await ObterNomeTurma(pendencia),
                //    Bimestre = await ObterBimestreTurma(pendencia)
                //});
                listaPendenciasDto.AddRange(await ObterTextoPendencias(pendencia, usuarioLogado.CodigoRf));
            }

            return listaPendenciasDto;
        }

        private async Task<IEnumerable<PendenciaDto>> ObterTextoPendencias(Pendencia pendencia, string codigoRf)
        {
            return /*pendencia.EhPendenciaAula() ?
                        await ObterDescricaoTurmaPendenciaAula(pendencia.Id) :
                    pendencia.EhPendenciaFechamento() ?
                        await ObterDescricaoTurmaPendenciaFechamento(pendencia.Id) :
                    pendencia.EhPendenciaProfessor() ?
                        await ObterDescricaoTurmaPendenciaProfessor(pendencia.Id) :*/
                    //pendencia.EhPendenciaDiarioBordo() ?
                        await ObterTextoPendenciaDiarioBordo(pendencia, codigoRf);
        }

        private async Task<IEnumerable<PendenciaDto>> ObterTextoPendenciaDiarioBordo(Pendencia pendencia, string codigoRf)
        {
            var pendenciasDiarioBordo = new List<PendenciaDto>();

            var pendenciasDiarios = await mediator.Send(new ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(pendencia.Id, codigoRf));

            var agrupamentoPendenciasBimestres = pendenciasDiarios.GroupBy(g => new { g.PendenciaId, g.Bimestre, g.ModalidadeCodigo, g.NomeTurma }, (key, group) =>
                                                        new { key.PendenciaId, key.Bimestre, key.ModalidadeCodigo, key.NomeTurma, Result = group.Select(s => s).ToList() });


            var descricao = new StringBuilder();

            foreach (var pendenciaBimestre in agrupamentoPendenciasBimestres)
            {
                descricao = new StringBuilder(pendencia.Descricao);

                descricao.AppendLine("<br /><ul>");

                var pendenciaFormatada = new PendenciaDto
                {
                    Titulo = !string.IsNullOrEmpty(pendencia.Titulo) ? pendencia.Titulo : pendencia.Tipo.Name(),
                    Tipo = pendencia.Tipo.GroupName(),
                    Bimestre = ObterNomeBimestre(pendenciaBimestre.Bimestre),
                    Turma = $"{((Modalidade)pendenciaBimestre.ModalidadeCodigo).ShortName()} - {pendenciaBimestre.NomeTurma}"
                };

                foreach (var pendenciaDetalhe in pendenciaBimestre.Result)
                    descricao.AppendLine($"<li>{pendenciaDetalhe.DataAula:dd/MM}</li>");

                descricao.AppendLine("</ul>");
                descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

                pendenciaFormatada.Detalhe = descricao.ToString();
                pendenciasDiarioBordo.Add(pendenciaFormatada);

            }
            return pendenciasDiarioBordo;
        }

        private async Task<string> ObterBimestreTurma(Pendencia pendencia)
        {
            Turma turma = pendencia.EhPendenciaAula() ?
                         await mediator.Send(new ObterTurmaDaPendenciaAulaQuery(pendencia.Id)) :
                    pendencia.EhPendenciaFechamento() ?
                         await mediator.Send(new ObterTurmaDaPendenciaFechamentoQuery(pendencia.Id)) :
                    pendencia.EhPendenciaProfessor() ?
                        await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendencia.Id)) :
                    pendencia.EhPendenciaDiarioBordo()?
                         await mediator.Send(new ObterTurmaDaPendenciaDiarioQuery(pendencia.Id)) :
                        null;

            if (turma == null)
                return "";
            else
                return await ObterDescricaoBimestrePendencia(pendencia.Id, turma.Id, pendencia.CriadoEm);
        }

        private async Task<string> ObterDescricaoTurmaPendenciaFechamento(long pendenciaId)
            => ObterNomeTurma(await mediator.Send(new ObterTurmaDaPendenciaFechamentoQuery(pendenciaId)));

        private async Task<string> ObterDescricaoTurmaPendenciaAula(long pendenciaId)
            => ObterNomeTurma(await mediator.Send(new ObterTurmaDaPendenciaAulaQuery(pendenciaId)));

        private async Task<string> ObterDescricaoTurmaPendenciaDiarioBordo(long pendenciaId)
            => ObterNomeTurma(await mediator.Send(new ObterTurmaDaPendenciaDiarioQuery(pendenciaId)));

        private async Task<string> ObterDescricaoTurmaPendenciaProfessor(long pendenciaId)
            => ObterNomeTurma(await mediator.Send(new ObterTurmaDaPendenciaProfessorQuery(pendenciaId)));

        private async Task<string> ObterDescricaoBimestrePendencia(long pendenciaId, long turmaId, DateTime dataPendenciaCriada)
        {
            int bimestre = await mediator.Send(new ObterModalidadePorPendenciaQuery(pendenciaId, turmaId, dataPendenciaCriada));
            return ObterNomeBimestre(bimestre);
        }

        private string ObterNomeTurma(Turma turma)
            => turma != null ? $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}" : "";

        private string ObterNomeBimestre(int bimestre)
            => bimestre == 0 ? "Final" : $"{bimestre}º Bimestre";

        private async Task<string> ObterDescricaoPendencia(Pendencia pendencia, string codigoRfProfessor)
        {
            if (pendencia.EhPendenciaAula())
                return await ObterDescricaoPendenciaAula(pendencia);
            if (pendencia.EhPendenciaCadastroEvento())
                return await ObterDescricaoPendenciaEvento(pendencia);
            if (pendencia.EhPendenciaAusenciaAvaliacaoCP())
                return await ObterDescricaoPendenciaAusenciaAvaliacaoCP(pendencia);
            if (pendencia.EhPendenciaAusenciaDeRegistroIndividual())
                return await ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(pendencia);
            if (pendencia.EhPendenciaDiarioBordo())
                return await ObterDescricaoPendenciaDiarioBordo(pendencia, codigoRfProfessor);


            return ObterDescricaoPendenciaGeral(pendencia);
        }

        private async Task<string> ObterDescricaoPendenciaAusenciaAvaliacaoCP(Pendencia pendencia)
        {
            var pendenciasProfessor = await mediator.Send(new ObterPendenciasProfessorPorPendenciaIdQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.Append("<br/><table style='margin-left: auto; margin-right: auto; margin-top: 10px' border='2' cellpadding='5'>");
            descricao.Append("<tr>");
            descricao.Append("<td style='padding: 5px;'><b>Componente curricular</b></td>");
            descricao.Append("<td style='padding: 5px;'><b>Professor titular</b></td>");
            descricao.Append("</tr>");
            foreach (var pendenciaProfessor in pendenciasProfessor)
            {
                descricao.Append("<tr style='padding:5px'>");
                descricao.Append($"<td style='padding: 5px;'>{pendenciaProfessor.ComponenteCurricular}</td>");
                descricao.Append($"<td style='padding: 5px;'>{pendenciaProfessor.Professor}({pendenciaProfessor.ProfessorRf})</td>");
                descricao.Append("</tr>");
            }
            descricao.Append("</table><br/>");
            descricao.Append($"<b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private string ObterDescricaoPendenciaGeral(Pendencia pendencia)
        {
            return $"{pendencia.Descricao}<br /><br/><b>{pendencia.Instrucao}</b>";
        }

        private async Task<string> ObterDescricaoPendenciaEvento(Pendencia pendencia)
        {
            var pendenciasEventos = await mediator.Send(new ObterPendenciasParametroEventoPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul>");

            foreach (var pendenciaEvento in pendenciasEventos)
            {
                descricao.AppendLine($"<li>{pendenciaEvento.Descricao} ({pendenciaEvento.Valor})</li>");
            }
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private async Task<string> ObterDescricaoPendenciaAula(Pendencia pendencia)
        {
            var pendenciasAulas = await mediator.Send(new ObterPendenciasAulasPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul>");

            foreach (var pendenciaAula in pendenciasAulas)
            {
                descricao.AppendLine($"<li>{pendenciaAula.DataAula:dd/MM/yyyy} {ObterComplementoDescricao(pendenciaAula)}</li>");
            }
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private string ObterComplementoDescricao(PendenciaAulaDto pendenciaAula)
        {
            return !string.IsNullOrEmpty(pendenciaAula.TituloAvaliacao)? $" - {pendenciaAula.TituloAvaliacao}" : ObterComplementoReposicao(pendenciaAula.EhReposicao);
        }

        private string ObterComplementoReposicao(bool ehReposicao)
        {
            return ehReposicao ? " - Reposição" : "";
        }

        private async Task<string> ObterDescricaoPendenciaDiarioBordo(Pendencia pendencia, string codigoRfProfessor)
        {
            var pendenciasDiarios = await mediator.Send(new ObterPendenciasDiarioPorPendenciaIdEProfessorQuery(pendencia.Id, codigoRfProfessor));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul>");

            foreach (var pendenciaDiario in pendenciasDiarios)
                descricao.AppendLine($"<li>{pendenciaDiario.DataAula:dd/MM/yyyy} {ObterComplementoReposicao(pendenciaDiario.EhReposicao)}</li>");

            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }

        private async Task<string> ObterDescricaoPendenciaAusenciaRegistroIndividualAsync(Pendencia pendencia)
        {
            var alunos = await mediator.Send(new ObterPendenciaRegistroIndividualCodigosAlunosPorPendenciaQuery(pendencia.Id));

            var descricao = new StringBuilder(pendencia.Descricao);
            descricao.AppendLine("<br /><ul style='padding-top: 20px; padding-left: 20px';>");

            foreach (var aluno in alunos)
            {
                descricao.AppendLine($"<li style='padding-top: 2px;'>{aluno.NomeValido()} ({aluno.CodigoAluno})</li>");
            }
            descricao.AppendLine("</ul>");
            descricao.AppendLine($"<br/><b>{pendencia.Instrucao}</b>");

            return descricao.ToString();
        }
    }
}