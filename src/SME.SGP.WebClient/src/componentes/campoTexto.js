import React from 'react';
import { Field, ErrorMessage } from 'formik';

import styled from 'styled-components';
import { Base } from './colors';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
    background-image: none !important;
  }
  .disabled {
    background: transparent !important;
    border-color: ${Base.CinzaDesabilitado} !important;
    color: ${Base.CinzaDesabilitado} !important;
  }
`;

const CampoTexto = ({ name, id, form, className, type, desabilitado }) => {
  const possuiErro = () => {
    return form.errors[name] && form.touched[name];
  };

  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && 'button' === relatedTarget.getAttribute('type')) {
      event.preventDefault();
    }
  };

  return (
    <Campo erro={possuiErro()}>
      <Field
        name={name}
        id={id || name}
        className={`form-control campo ${
          possuiErro() ? 'is-invalid' : ''
        } ${className || ''} ${desabilitado ? 'desabilitado' : ''}`}
        component={type || 'text'}
        disabled={desabilitado}
        onBlur={executaOnBlur}
      />
      {possuiErro() && <span>{form.errors[name]}</span>}
    </Campo>
  );
};
export default CampoTexto;
