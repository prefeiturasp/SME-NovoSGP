import { Input } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';

const Container = styled.div`
  .ant-input {
    height: 38px;
  }
`;

const InputComponent = props => {
  const { placeholder } = props;

  return (
    <Container>
      <Input placeholder={placeholder} />
    </Container>
  );
};

InputComponent.propTypes = {
  placeholder: PropTypes.string,
};

InputComponent.defaultProps = {
  placeholder: '',
};

export default InputComponent;
