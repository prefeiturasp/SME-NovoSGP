import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';

const Container = styled.div`
  label {
    position: relative;
    color: rgba(0, 0, 0, 0.25);
    font-size: 14px;
    background: #fff;
  }
`;

const LabelSemDados = ({ text, center }) => {
  return (
    <Container className={center && 'text-center'}>
      <label id={shortid.generate()}>{text}</label>
    </Container>
  );
};
LabelSemDados.propTypes = {
  text: PropTypes.string,
  center: PropTypes.bool,
};

LabelSemDados.defaultProps = {
  text: PropTypes.string,
  center: false,
};

export default LabelSemDados;
