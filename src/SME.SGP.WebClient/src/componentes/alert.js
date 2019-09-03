import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Base, Active, Hover } from './colors';
import { removerAlerta } from '../redux/modulos/alertas/actions';
import { useDispatch } from 'react-redux';
const Alert = props => {

  const { tipo, id, mensagem } = props.alerta;

  const {className} = props;

  const dispatch = useDispatch();

  return (
    <div
      className={`alert alert-${tipo} alert-dismissible fade show text-center ${className}`}
      role="alert"
    >
      <b>{`${mensagem}`}</b>
      <button
        type="button"
        className="close"
        onClick={() => dispatch(removerAlerta(id))}
        aria-label="Close"
      >
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
  );
};

export default Alert;
