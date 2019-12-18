import Servico from '../../../servicos/Paginas/PlanoAnualServices';
import { sucesso, erro } from '../../../servicos/alertas';
import filtroPlanoAnualDto from '~/dtos/filtroPlanoAnualDto';

export function Salvar(indice, bimestre) {
  return {
    type: '@bimestres/SalvarBimestre',
    payload: {
      indice,
      bimestre,
    },
  };
}

export function SalvarBimestres(bimestres) {
  return {
    type: '@bimestres/SalvarTodosBimestres',
    payload: bimestres,
  };
}

export function SalvarDisciplinasPlanoAnual(disciplinas) {
  return {
    type: '@bimestres/SalvarDisciplinasPlanoAnual',
    payload: disciplinas,
  };
}

export function SelecionarDisciplinaPlanoAnual(codigo) {
  return {
    type: '@bimestres/SelecionarDisciplinaPlanoAnual',
    payload: {
      codigo,
    },
  };
}

export function RemoverFocado() {
  return {
    type: '@bimestres/RemoverFocado',
  };
}

export function LimparDisciplinaPlanoAnual() {
  return {
    type: '@bimestres/LimparDisciplinaPlanoAnual',
  };
}

export function LimparBimestres() {
  return {
    type: '@bimestres/LimparBimestres',
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
      .filter(materia => materia.selecionada)
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

export function PosPost(recarregar) {
  return {
    type: '@bimestres/PosPostBimestre',
    payload: recarregar,
  };
}

export function Post(bimestres, codigoDisciplinaPlanoAnual) {
  return dispatch => {
    Servico.postPlanoAnual(bimestres, codigoDisciplinaPlanoAnual)
      .then(() => {
        requestAnimationFrame(() => dispatch(setNaoEdicao()));
        sucesso('Suas informações foram salvas com sucesso.');
        dispatch(PosPost(true));
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

        dispatch(PosPost(false));
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

export function removerSelecaoTodosObjetivos(indice) {
  return {
    type: '@bimestres/removerSelecaoTodosObjetivos',
    payload: {
      indice,
    },
  };
}

export function ObterBimestreServidor(
  Bimestre,
  disciplinaSelecionada,
  layoutEspecial
) {
  const filtro = new filtroPlanoAnualDto(
    Bimestre.anoLetivo,
    Bimestre.indice,
    Bimestre.escolaId,
    Bimestre.turmaId,
    disciplinaSelecionada
  );

  return dispatch => {
    Servico.obterBimestre(filtro)
      .then(res => {
        const bimestreDTO = res.data;

        dispatch(
          Salvar(Bimestre.indice, {
            ...Bimestre,
            objetivo: bimestreDTO.descricao,
            ehExpandido: true,
            id: bimestreDTO.id,
            alteradoPor: bimestreDTO.alteradoPor,
            alteradoEm: bimestreDTO.alteradoEm,
            alteradoRF: bimestreDTO.alteradoRF,
            criadoRF: bimestreDTO.criadoRF,
            criadoEm: bimestreDTO.criadoEm,
            LayoutEspecial: bimestreDTO.migrado || layoutEspecial,
            migrado: bimestreDTO.migrado,
            criadoPor: bimestreDTO.criadoPor,
            objetivosAprendizagem: bimestreDTO.objetivosAprendizagem.map(
              obj => {
                obj.selected = true;
                return obj;
              }
            ),
          })
        );
      })
      .catch(() => {
        erro('Não foi possivel obter os dados do periodo selecionado');
      });
  };
}

const _getStringPor = (responsavelPor, dataEM, operacao, rf) => {
  const data = new Date(dataEM);
  const dia = data
    .getDate()
    .toString()
    .padStart(2, '0');

  const mes = (data.getMonth() + 1).toString().padStart(2, '0');
  const ano = data.getFullYear();
  const hora = data
    .getHours()
    .toString()
    .padStart(2, '0');

  const minuto = data
    .getMinutes()
    .toString()
    .padStart(2, '0');

  const segundos = data
    .getSeconds()
    .toString()
    .padStart(2, '0');

  return `${operacao} por ${responsavelPor} ${rf} em ${dia}/${mes}/${ano} ${hora}:${minuto}:${segundos}`;
};
