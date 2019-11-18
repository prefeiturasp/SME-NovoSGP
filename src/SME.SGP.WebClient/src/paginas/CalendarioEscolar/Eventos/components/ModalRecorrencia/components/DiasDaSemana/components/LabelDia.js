import React from 'react';
import PropTypes from 'prop-types';

// Styles
import styled from 'styled-components';
import { Base } from '~/componentes/colors';

const Circulo = styled.a`
    padding: 8px 14px;
    border-radius: 40px;
    border: solid 1px #e8e8e8;
    background-color: #ffffff;
    border-radius: 50%;
    margin: .2rem;
    color: ${Base.CinzaBotao}

    &:hover {
        border: solid 1px #e8e8e8;
        background-color: ${Base.Roxo};
        color: white !important;
    }

    &.active {
        border: solid 1px #e8e8e8;
        background-color: ${Base.Roxo};
        color: white !important;
    }
`;

function LabelDia({ isActive, onChange, data }) {
  return (
    <Circulo className={isActive && 'active'} onClick={() => onChange(data)}>
      {data.legenda}
    </Circulo>
  );
}

LabelDia.defaultProps = {
  data: {
    label: '',
    value: '',
  },
  onChange: () => {},
  isActive: false,
};

LabelDia.propTypes = {
  data: PropTypes.objectOf(PropTypes.any),
  onChange: PropTypes.func,
  isActive: PropTypes.bool,
};

export default LabelDia;
