import React from 'react';
import { Field } from 'formik';

import styled from 'styled-components';
import { Input } from 'antd';
import { Base } from './colors';

import Label from './label';

const Campo = styled.div`
  span {
    color: ${Base.Vermelho};
  }
  .campo {
    margin-bottom: 5px;
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
  classNameCampo,
  type,
  maskType,
  placeholder,
  onChange,
  value,
  desabilitado,
  maxlength,
  label
}) => {

  const possuiErro = () => {
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
      <Campo className={classNameCampo}>
        {label ? <Label text={label} control={name || ''} /> : ''}
        {form ? (
          <>
            {' '}
            <Field
              name={name}
              id={id || name}
              className={`form-control campo ${
                possuiErro() ? 'is-invalid' : ''
              } ${className || ''} ${desabilitado ? 'desabilitado' : ''}`}
              component={type || 'input'}
              type = {maskType && maskType}
              disabled={desabilitado}
              onBlur={executaOnBlur}
              maxLength={maxlength || ''}
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
