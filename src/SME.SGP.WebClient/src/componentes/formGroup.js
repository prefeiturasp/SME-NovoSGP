import React from 'react';
import Styled from 'styled-components';

const FormGroup = props => {
  const { children, className } = props;

  const Container = Styled.div`
  margin-bottom: .9rem !important;
  `;

  return (
    <Container className={`form-group ${className}`}>{children}</Container>
  );
};

export default FormGroup;
