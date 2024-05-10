import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setQuestionarioDinamicoExpandirLinhaAusenciaEstudante } from '~/redux/modulos/questionarioDinamico/actions';
import { ContainerColunaMotivoAusencia } from './informacoesEscolares.css';

const BtnExpandirAusenciaEstudante = props => {
  const dispatch = useDispatch();

  const expandirLinhaAusenciaEstudante = useSelector(
    store =>
      store.questionarioDinamico
        .questionarioDinamicoExpandirLinhaAusenciaEstudante
  );

  const { indexLinha } = props;

  const onClickExpandir = index => {
    let novaLinha = [];
    if (expandirLinhaAusenciaEstudante[index]) {
      expandirLinhaAusenciaEstudante[index] = false;
      novaLinha = expandirLinhaAusenciaEstudante;
    } else {
      novaLinha[index] = true;
    }

    dispatch(
      setQuestionarioDinamicoExpandirLinhaAusenciaEstudante([...novaLinha])
    );
  };

  return (
    <ContainerColunaMotivoAusencia
      onClick={() => onClickExpandir(indexLinha)}
      className={
        expandirLinhaAusenciaEstudante[indexLinha]
          ? 'fas fa-chevron-up'
          : 'fas fa-chevron-down'
      }
    />
  );
};

BtnExpandirAusenciaEstudante.defaultProps = {
  indexLinha: PropTypes.number,
};

BtnExpandirAusenciaEstudante.propTypes = {
  indexLinha: null,
};

export default BtnExpandirAusenciaEstudante;
