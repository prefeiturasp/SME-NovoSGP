import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

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
`;

const Label = ({ text, control, center, className }) => {
  return (
    <Container className={center && 'text-center'}>
      <label htmlFor={control} id={text} className={className}>
        {text}
      </label>
    </Container>
  );
};
Label.propTypes = {
  text: PropTypes.string,
  control: PropTypes.string,
  center: PropTypes.bool,
};

Label.defaultProps = {
  text: PropTypes.string,
  control: null,
  center: false,
};

export default Label;
