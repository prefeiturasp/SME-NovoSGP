import React from 'react';
import { Input } from '@rocketseat/unform';
import { Base } from './colors';

import styled from 'styled-components';
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
