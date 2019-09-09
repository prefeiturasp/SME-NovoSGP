import Servico from '../../../servicos/Paginas/PlanoAnualServices';
import { sucesso, erro } from '../../../servicos/alertas';

export function Salvar(indice, bimestre) {
  return {
    type: '@bimestres/SalvarBimestre',
    payload: {
      indice,
      bimestre,
    },
  };
}

export function SalvarMaterias(indice, materias) {
  return {
    type: '@bimestres/SalvarMateria',
    payload: {
      indice,
      materias,
    },
  };
}

export function SalvarEhExpandido(indice, ehExpandido) {
  return {
    type: '@bimestres/SalvarEhExpandido',
    payload: {
      indice,
      ehExpandido,
    },
  };
}

export function SalvarObjetivos(indice, objetivos) {
  return {
    type: '@bimestres/SalvarObjetivos',
    payload: {
      indice,
      objetivos,
    },
  };
}

export function SetarDescricaoFunction(indice, setarObjetivo) {
  return {
    type: '@bimestres/SetarDescricaoFunction',
    payload: {
      indice,
      setarObjetivo,
    },
  };
}

export function SelecionarMateria(indice, indiceMateria, selecionarMateria) {
  return {
    type: '@bimestres/SelecionarMateria',
    payload: {
      indice,
      indiceMateria,
      selecionarMateria,
    },
  };
}

export function SelecionarObjetivo(indice, indiceObjetivo, selecionarObjetivo) {
  return {
    type: '@bimestres/SelecionarObjetivo',
    payload: {
      indice,
      indiceObjetivo,
      selecionarObjetivo,
    },
  };
}

export function SetarDescricao(indice, descricao) {
  return {
    type: '@bimestres/SetarDescricao',
    payload: {
      indice,
      descricao,
    },
  };
}

export function ObterObjetivosCall(bimestre) {
  return dispatch => {
    if (!bimestre.materias || bimestre.materias.length === 0) {
      dispatch(SalvarObjetivos(bimestre.indice, []));
      return;
    }

    const objetivosSelecionados = bimestre.objetivosAprendizagem
      ? bimestre.objetivosAprendizagem.filter(obj => obj.selected)
      : [];

    const materiasSelecionadas = bimestre.materias
      .filter(materia => materia.selected)
      .map(x => x.codigo);

    if (!materiasSelecionadas || materiasSelecionadas.length === 0) {
      dispatch(
        SalvarObjetivos(bimestre.indice, [].concat(objetivosSelecionados))
      );
      return;
    }

    dispatch(SalvarEhExpandido(bimestre.indice, true));

    Servico.getObjetivoseByDisciplinas(
      bimestre.anoEscolar,
      materiasSelecionadas
    ).then(res => {
      if (
        !bimestre.objetivosAprendizagem ||
        bimestre.objetivosAprendizagem.length === 0
      ) {
        dispatch(
          SalvarObjetivos(bimestre.indice, res.concat(objetivosSelecionados))
        );
        return;
      }

      const Aux = [...objetivosSelecionados, ...res];

      const concatenados = Aux.concat(
        res.filter(item => {
          const index = bimestre.objetivosAprendizagem.findIndex(
            x => x.codigo === item.codigo
          );

          return index < 0;
        })
      );

      dispatch(SalvarObjetivos(bimestre.indice, concatenados));
    });
  };
}

export function PrePost() {
  return {
    type: '@bimestres/PrePostBimestre',
  };
}

export function PosPost() {
  return {
    type: '@bimestres/PosPostBimestre',
  };
}

export function Post(bimestres) {
  return dispatch => {
    Servico.postPlanoAnual(bimestres)
      .then(() => {
        requestAnimationFrame(() => dispatch(setNaoEdicao()));
        sucesso('Suas informações foram salvas com sucesso.');
      })
      .catch(err => {
        dispatch(
          setBimestresErro({
            type: 'erro',
            content: err.error,
            title: 'Ocorreu uma falha',
            onClose: () => dispatch(setLimpartBimestresErro()),
            visible: true,
          })
        );
      })
      .finally(() => {
        dispatch(PosPost());
      });
  };
}

export function setBimestresErro(bimestresErro) {
  return {
    type: '@bimestres/BimestresErro',
    payload: bimestresErro,
  };
}

export function setLimpartBimestresErro() {
  return {
    type: '@bimestres/LimparBimestresErro',
    payload: {
      type: '',
      content: [],
      title: '',
      onClose: null,
      visible: false,
    },
  };
}

export function setNaoEdicao() {
  return {
    type: '@bimestres/setEdicaoFalse',
  };
}

export function setJaSincronizou() {
  return {
    type: '@bimestres/jaSincronizou',
    payload: true,
  };
}

export function ObterBimestreServidor(Bimestre) {
  return dispatch => {
    Servico.obterBimestre({
      AnoLetivo: Bimestre.anoLetivo,
      Bimestre: Bimestre.indice,
      EscolaId: Bimestre.escolaId,
      TurmaId: Bimestre.turmaId,
    })
      .then(res => {
        const bimestreDTO = res.data;

        dispatch(
          Salvar(Bimestre.indice, {
            ...Bimestre,
            objetivo: bimestreDTO.descricao,
            ehExpandido: true,
            objetivosAprendizagem: bimestreDTO.objetivosAprendizagem.map(
              obj => {
                obj.selected = true;
                return obj;
              }
            ),
          })
        );

        sucesso('Dados obtidos com sucesso');
      })
      .catch(() => {
        erro('Não foi possivel obter os dados do periodo selecionado');
      });
  };
}
