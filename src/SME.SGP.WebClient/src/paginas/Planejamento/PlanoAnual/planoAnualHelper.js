import FiltroPlanoAnualDto from '~/dtos/filtroPlanoAnualDto';
import Service from '~/servicos/Paginas/PlanoAnualServices';
import _ from 'lodash';
import { erro } from '~/servicos/alertas';

export default class PlanoAnualHelper {
  static async verificarSeExiste(filtroPlanoAnualDto, ehEja) {
    const validarExistente = await Service.validarPlanoExistente(
      filtroPlanoAnualDto
    )
      .then(res => res)
      .catch(() => {
        erro(
          `Não foi possivel obter os dados do ${
            ehEja ? 'plano semestral' : 'plano anual'
          }`
        );
        return false;
      });

    return validarExistente;
  }

  static async ObterBimestreExpandido(filtroPlanoAnualExpandidoDto) {
    console.log(filtroPlanoAnualExpandidoDto);

    return await Service.obterBimestreExpandido(filtroPlanoAnualExpandidoDto)
      .then(res => {
        return {
          sucesso: true,
          bimestre: res.data,
        };
      })
      .catch(err => {
        return {
          sucesso: false,
          response: err.response ? err.response : null,
        };
      });
  }

  static async ObterDisciplinasPlano(codigoRf, turmaId) {
    const disciplinas = await Service.getDisciplinasProfessor(codigoRf, turmaId)
      .then(res => res)
      .catch(() => {
        erro(`Não foi possivel obter as disciplinas do professor`);
        return null;
      });

    return disciplinas;
  }

  static async ObterDiscplinasObjetivos(turmaId, disciplinaSelecionada) {
    const disciplinas = await Service.getDisciplinasProfessorObjetivos(
      turmaId,
      disciplinaSelecionada
    )
      .then(res => res)
      .catch(() => {
        erro(`Não foi possivel obter as disciplinas do professor`);
        return null;
      });

    if (disciplinaSelecionada.regencia) return disciplinas;

    const retorno =
      disciplinas &&
      disciplinas.find(x => x.codigo === disciplinaSelecionada.codigo);

    return [retorno];
  }

  static ObtenhaBimestres = (
    disciplinas = [],
    ehEdicao,
    filtroPlanoAnualDto,
    anoEscolar,
    layoutEspecial,
    ehEja
  ) => {
    let bimestres = [];

    const qtdBimestres = ehEja ? 2 : 4;

    for (let i = 1; i <= qtdBimestres; i++) {
      const Nome = `${i}º Bimestre`;

      const objetivo = '';

      const bimestre = {
        anoLetivo: filtroPlanoAnualDto.anoLetivo,
        anoEscolar: anoEscolar,
        escolaId: filtroPlanoAnualDto.escolaId,
        turmaId: filtroPlanoAnualDto.turmaId,
        indice: i,
        nome: Nome,
        materias: disciplinas,
        objetivo: objetivo,
        paraEnviar: false,
        recarregarPlanoAnual: false,
        ehEdicao: !ehEdicao,
        LayoutEspecial: layoutEspecial,
        ehExpandido: !ehEdicao,
        jaSincronizou: false,
      };

      bimestres[i] = bimestre;
    }

    return bimestres;
  };

  static async ObtenhaTurmasCopiarConteudo(
    anoLetivo,
    escolaId,
    turmaId,
    disciplinaSelecionada,
    ehEja,
    usuario,
    turmaSelecionada
  ) {
    const filtroEdicao = new FiltroPlanoAnualDto(
      anoLetivo,
      1,
      escolaId,
      turmaId,
      disciplinaSelecionada.codigo
    );

    const existePlano = await PlanoAnualHelper.verificarSeExiste(
      filtroEdicao,
      ehEja
    );

    if (!existePlano) {
      erro('Este plano ainda não foi salvo na base de dados');
      return null;
    }

    const turmasCopiarConteudo = await PlanoAnualHelper.ObtenhaTumasIrmas(
      usuario,
      turmaSelecionada,
      disciplinaSelecionada,
      anoLetivo,
      escolaId
    );

    if (!turmasCopiarConteudo) {
      return null;
    }

    if (turmasCopiarConteudo.length === 0) {
      erro('Nenhuma turma elegivel para copiar o conteudo');
      return null;
    }

    const turmasComDisciplinasIguais = await PlanoAnualHelper.ValidarDisciplinasIguais(
      turmasCopiarConteudo,
      disciplinaSelecionada,
      usuario
    );

    if (!turmasComDisciplinasIguais.sucesso) {
      if (!turmasComDisciplinasIguais.erroTratado)
        erro('Não há nenhuma turma elegivel para receber a copia deste plano');
      return null;
    }

    return turmasComDisciplinasIguais.turmasCopiarConteudo;
  }

  static async ObtenhaTumasIrmas(
    usuario,
    turmaSelecionada,
    disciplinaSelecionada,
    anoLetivo,
    escolaId
  ) {
    const turmasIrmas = usuario.turmasUsuario.filter(
      turma =>
        turma.ano === turmaSelecionada[0].ano &&
        turma.codEscola === turmaSelecionada[0].codEscola &&
        turma.codigo !== turmaSelecionada[0].codTurma
    );

    const promissesTurmas = [];

    turmasIrmas.forEach(turma => {
      const filtro = new FiltroPlanoAnualDto(
        anoLetivo,
        1,
        escolaId,
        turma.codigo,
        disciplinaSelecionada.codigo
      );

      const promise = Service.validarPlanoExistente(filtro);

      promissesTurmas.push(promise);
    });

    return Promise.all(promissesTurmas)
      .then(res => {
        res.forEach((resposta, indice) => {
          if (resposta) turmasIrmas[indice].temPlano = resposta;
        });

        return turmasIrmas;
      })
      .catch(() => {
        erro('Não foi possivel obter as disciplinas elegiveis');
        return null;
      });
  }

  static async ValidarDisciplinasIguais(
    turmasCopiarConteudo,
    disciplinaSelecionada,
    usuario
  ) {
    const promissesTurmas = [];

    for (let i = 0; i < turmasCopiarConteudo.length; i++) {
      promissesTurmas.push(
        Service.getDisciplinasProfessor(
          usuario.rf,
          turmasCopiarConteudo[i].codigo
        )
      );
    }

    return Promise.all(promissesTurmas)
      .then(resultados => {
        for (let i = 0; i < resultados.length; i++) {
          const disciplinasResultado = resultados[i];

          disciplinasResultado.forEach(disciplina => {
            if (disciplina.codigo === disciplinaSelecionada.codigo)
              turmasCopiarConteudo[i].disponivelCopia = true;
          });
        }

        const temTurmaElegivel =
          turmasCopiarConteudo.filter(turma => turma.disponivelCopia).length >
          0;

        return {
          sucesso: temTurmaElegivel,
          erroTratado: false,
          erro: !temTurmaElegivel
            ? 'Não há nenhuma turma elegivel para receber a copia deste plano'
            : '',
          turmasCopiarConteudo: turmasCopiarConteudo.filter(
            turma => turma.disponivelCopia
          ),
        };
      })
      .catch(() => {
        erro('Não foi possivel obter as turmas disponiveis');
        return {
          sucesso: false,
          erroTratado: true,
        };
      });
  }

  static async ObtenhaBimestresCopiarConteudo(
    anoLetivo,
    escolaId,
    turmaId,
    qtdBimestres,
    disciplinaSelecionada
  ) {
    const promissesBimestres = [];

    for (let i = 1; i <= qtdBimestres; i++) {
      const filtro = new FiltroPlanoAnualDto(
        anoLetivo,
        i,
        escolaId,
        turmaId,
        disciplinaSelecionada.codigo
      );

      const promise = Service.obterBimestre(filtro);

      promissesBimestres.push(promise);
    }

    const bimestresCopiar = await Promise.all(promissesBimestres)
      .then(resultados => resultados.map(res => res.data))
      .catch(() => null);

    return bimestresCopiar;
  }

  static TratarBimestresCopiarConteudo(bimestresCopiar, disciplinaSelecionada) {
    const PlanoAnualEnviar = {
      Id: bimestresCopiar[0].id,
      AnoLetivo: bimestresCopiar[0].anoLetivo,
      EscolaId: bimestresCopiar[0].escolaId,
      TurmaId: bimestresCopiar[0].turmaId,
      ComponenteCurricularEolId: disciplinaSelecionada.codigo,
      Bimestres: [],
    };

    bimestresCopiar.forEach((bimestre, index) => {
      PlanoAnualEnviar.Bimestres.push({
        Bimestre: index + 1,
        ObjetivosAprendizagem: bimestre.objetivosAprendizagem,
        Descricao: bimestre.descricao,
      });
    });

    return PlanoAnualEnviar;
  }

  static async CopiarConteudo(planoAnualEnviar, codigoRf, turmasDestino) {
    return await Service.copiarConteudo(
      planoAnualEnviar,
      codigoRf,
      turmasDestino
    )
      .then(() => {
        return { sucesso: true };
      })
      .catch(erro => {
        return { sucesso: false, erro };
      });
  }
}
