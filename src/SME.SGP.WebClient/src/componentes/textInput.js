import React from 'react';
import { Input } from '@rocketseat/unform';
import styled from 'styled-components';
import { Base } from './colors';

const CampoTexto = styled.div`
  span {
    color: ${Base.Vermelho};
  }
`;

const TextInput = ({ name, multiline = false, className }) => {
  return (
    <CampoTexto>
      <Input
        name={name}
        multiline={multiline}
        className={`form-control ${className}`}
      />
    </CampoTexto>
  );
};
export default TextInput;
