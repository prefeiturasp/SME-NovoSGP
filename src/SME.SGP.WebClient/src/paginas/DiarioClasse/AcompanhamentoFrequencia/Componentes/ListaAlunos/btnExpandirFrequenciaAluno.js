import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setExpandirLinhaFrequenciaAluno } from '~/redux/modulos/acompanhamentoFrequencia/actions';
import { ContainerColunaMotivoAusencia } from './listaAlunos.css';

const BtnExpandirFrequenciaAluno = props => {
  const dispatch = useDispatch();

  const expandirLinhaFrequenciaAluno = useSelector(
    store => store.acompanhamentoFrequencia.expandirLinhaFrequenciaAluno
  );

  const { indexLinha } = props;

  const onClickExpandir = index => {
    let novaLinha = [];
    if (expandirLinhaFrequenciaAluno[index]) {
      expandirLinhaFrequenciaAluno[index] = false;
      novaLinha = expandirLinhaFrequenciaAluno;
    } else {
      novaLinha[index] = true;
    }

    dispatch(setExpandirLinhaFrequenciaAluno([...novaLinha]));
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
};

BtnExpandirFrequenciaAluno.propTypes = {
  indexLinha: null,
};

export default BtnExpandirFrequenciaAluno;
