import React from 'react';
import { Field, ErrorMessage } from 'formik';

import styled from 'styled-components';
import { Input } from 'antd';
import { Base } from './colors';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
    background-image: none !important;
  }
  .ant-input {
    height: 38px;
  }
`;

const CampoTexto = ({
  name,
  id,
  form,
  className,
  type,
  placeholder,
  onChange,
  value,
  desabilitado,
}) => {
  const possuiErro = () => {
    console.log(form && form.errors[name] && form.touched[name]);
    return form && form.errors[name] && form.touched[name];
  };
  const executaOnBlur = event => {
    const { relatedTarget } = event;
    if (relatedTarget && relatedTarget.getAttribute('type') === 'button') {
      event.preventDefault();
    }
  };
  return (
    <>
      <Campo>
        {form ? (
          <>
            {' '}
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
            <span>{form.errors[name]}</span>
          </>
        ) : (
          <Input placeholder={placeholder} onChange={onChange} value={value} />
        )}
      </Campo>
    </>
  );
};
export default CampoTexto;
