import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import {
  setExpandirLinhaFrequenciaAluno,
  setFrequenciaAlunoCodigo,
} from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { ContainerColunaMotivoAusencia } from './listaAlunos.css';

const BtnExpandirFrequenciaAluno = props => {
  const dispatch = useDispatch();

  const expandirLinhaFrequenciaAluno = useSelector(
    store => store.acompanhamentoFrequencia.expandirLinhaFrequenciaAluno
  );

  const { indexLinha, codigoAluno } = props;

  const onClickExpandir = index => {
    let novaLinha = [];
    let novoAluno = '';
    if (expandirLinhaFrequenciaAluno[index]) {
      expandirLinhaFrequenciaAluno[index] = false;
      novaLinha = expandirLinhaFrequenciaAluno;
      novoAluno = null;
    } else {
      novaLinha[index] = true;
      novoAluno = codigoAluno;
    }

    dispatch(setExpandirLinhaFrequenciaAluno([...novaLinha]));
    dispatch(setFrequenciaAlunoCodigo(novoAluno));
  };

  return (
    <ContainerColunaMotivoAusencia
      onClick={() => onClickExpandir(indexLinha)}
      className={
        expandirLinhaFrequenciaAluno[indexLinha]
          ? 'fas fa-chevron-up'
          : 'fas fa-chevron-down'
      }
    />
  );
};

BtnExpandirFrequenciaAluno.defaultProps = {
  indexLinha: PropTypes.number,
  codigoAluno: PropTypes.string,
};

BtnExpandirFrequenciaAluno.propTypes = {
  indexLinha: null,
  codigoAluno: PropTypes.string,
};

export default BtnExpandirFrequenciaAluno;
