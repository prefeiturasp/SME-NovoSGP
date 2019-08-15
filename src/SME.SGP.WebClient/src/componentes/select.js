import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';

const Container = styled.div`
  .custom-select {
    border-radius: 2px !important;
    font-weight: bold !important;
    outline: none !important;
    background-image: url("data:image/svg+xml;utf8,<svg aria-hidden='true' focusable='false' data-prefix='fas' data-icon='caret-down' class='svg-inline--fa fa-caret-down fa-w-10' role='img' xmlns='http://www.w3.org/2000/svg' viewBox='0 0 320 512'><path fill='currentColor' d='M31.3 192h257.3c17.8 0 26.7 21.5 14.1 34.1L174.1 354.8c-7.8 7.8-20.5 7.8-28.3 0L17.2 226.1C4.6 213.5 13.5 192 31.3 192z'></path></svg>") !important;
  }
`;

const Select = props => {
  const {
    name,
    id,
    className,
    onChange,
    label,
    value,
    lista,
  } = props;

  return (
    <Container>
      <select name={name} id={id} className={className} onChange={onChange}>
        {lista.map((item, indice) => {
          return (
            <option key={indice} value={`${item[value]}`}>
              {`${item[label]}`}
            </option>
          );
        })}
      </select>
    </Container>
  );
};

Select.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  className: PropTypes.string,
  onChange: PropTypes.func,
  label: PropTypes.string,
  value: PropTypes.string,
  lista: PropTypes.array,
};

export default Select;
