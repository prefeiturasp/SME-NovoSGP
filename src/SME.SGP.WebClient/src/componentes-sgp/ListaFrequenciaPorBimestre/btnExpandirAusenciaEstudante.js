import PropTypes from 'prop-types';
import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setExpandirLinhaAusenciaEstudante } from '~/redux/modulos/listaFrequenciaPorBimestre/actions';
import { ContainerColunaMotivoAusencia } from './listaFrequenciaPorBimestre.css';

const BtnExpandirAusenciaEstudante = props => {
  const dispatch = useDispatch();

  const expandirLinhaAusenciaEstudante = useSelector(
    store => store.listaFrequenciaPorBimestre.expandirLinhaAusenciaEstudante
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

    dispatch(setExpandirLinhaAusenciaEstudante([...novaLinha]));
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
  indexLinha: PropTypes.oneOfType([PropTypes.any]),
};

BtnExpandirAusenciaEstudante.propTypes = {
  indexLinha: '',
};

export default BtnExpandirAusenciaEstudante;
