import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { useDispatch } from 'react-redux';
import { Base, Active, Hover } from './colors';
import { removerAlerta } from '../redux/modulos/alertas/actions';

const Alert = props => {
  const { tipo, id, mensagem, estiloTitulo } = props.alerta;
  const { closable } = props;
  const { className } = props;

  const dispatch = useDispatch();

  return (
    <div
      className={`alert alert-${tipo} alert-dismissible fade show text-center ${className}`}
      role="alert"
    >
      <b style={estiloTitulo}>{mensagem}</b>
      {closable ? (
        <button
          type="button"
          className="close"
          onClick={() => dispatch(removerAlerta(id))}
          aria-label="Close"
        >
          <span aria-hidden="true">&times;</span>
        </button>
      ) : (
        ''
      )}
    </div>
  );
};

export default Alert;
