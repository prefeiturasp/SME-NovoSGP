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
`;

const Container = styled.div`
  .ant-input {
    height: 38px;
  }
`;

const CampoTexto = ({ name, id, form, className, type, placeholder, onChange, value }) => {
  const possuiErro = () => {
    return form.errors[name] && form.touched[name];
  };

  return (
    <>
      {form ? (
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
      ) : (
        <Container>
          <Input placeholder={placeholder}  onChange={onChange} value={value}/>
        </Container>
      )}
    </>
  );
};
export default CampoTexto;
