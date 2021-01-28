import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from './colors';

const Container = styled.div`
  label {
    height: 17px;
    font-family: Roboto;
    font-size: 14px;
    font-weight: normal;
    font-style: normal;
    font-stretch: normal;
    line-height: normal;
    letter-spacing: 0.14px;
    color: #42474a;
    font-weight: bold;
  }

  span {
    color: ${Base.CinzaBotao};
    font-weight: normal;
  }

  .campoOpcional {
    font-size: 12px !important;
    color: ${Base.CinzaBotao} !important;
    margin-left: 2px;
    font-weight: normal;
  }
`;

const Label = ({ text, control, center, className, campoOpcional, observacaoText }) => {
  return (
    <Container className={center && 'text-center'}>
      <label htmlFor={control} id={text} className={className}>
        {text}
        {observacaoText ? <span> {` ${observacaoText}`}</span> : ''}
      </label>
      {campoOpcional ? (
        <label htmlFor={control} id={text} className="campoOpcional">
          (opcional)
        </label>
      ) : null}
    </Container>
  );
};
Label.propTypes = {
  text: PropTypes.string,
  control: PropTypes.string,
  center: PropTypes.bool,
  className: PropTypes.string,
  observacaoText: PropTypes.string,
};

Label.defaultProps = {
  text: PropTypes.string,
  control: null,
  center: false,
  className: '',
  observacaoText: '',
};

export default Label;
