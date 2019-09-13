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
`;

const CampoTexto = ({ name, id, form, className, type }) => {
  const possuiErro = () => {
    return form.errors[name] && form.touched[name];
  };

  return (
    <Campo erro={possuiErro()}>
      <Field
        name={name}
        id={id || name}
        className={`form-control campo ${
          possuiErro() ? 'is-invalid' : ''
        } ${className || ''}`}
        component={type || 'text'}
      />
      {possuiErro() && <span>{form.errors[name]}</span>}
    </Campo>
  );
};
export default CampoTexto;
