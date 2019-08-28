import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import Select from 'antd/es/select';
import Icon from 'antd/es/icon';
import { Base } from './colors';

const Container = styled.div`
  font-weight: bold;
  .ant-select-arrow {
    color: ${Base.CinzaMako};
  }
  .ant-select-selection--single {
    align-items: center;
    display: flex;
    height: 38px;
  }
`;

const SelectComponent = props => {
  const {
    name,
    id,
    className,
    onChange,
    label,
    valueOption,
    valueSelect,
    lista,
  } = props;

  const { Option } = Select;

  return (
    <Container>
      <Select
        suffixIcon={<Icon type="caret-down" />}
        className={className}
        name={name}
        id={id}
        onChange={onChange}
        value={valueSelect}
      >
        {lista.length &&
          lista.map((item, indice) => {
            return (
              <Option key={indice} value={`${item[valueOption]}`}>
                {`${item[label]}`}
              </Option>
            );
          })}
      </Select>
    </Container>
  );
};

SelectComponent.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  className: PropTypes.string,
  onChange: PropTypes.func,
  label: PropTypes.string,
  valueOption: PropTypes.string,
  valueSelect: PropTypes.any,
  lista: PropTypes.array,
};

export default SelectComponent;
