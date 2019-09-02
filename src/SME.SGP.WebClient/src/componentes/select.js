import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import Select from 'antd/es/select';
import Icon from 'antd/es/icon';
import shortid from 'shortid';
import { Base } from './colors';

const Container = styled.div`
  .ant-select-arrow {
    color: ${Base.CinzaMako};
  }
  .ant-select-selection--single {
    align-items: center;
    display: flex;
    height: 38px;
  }
  .ant-select-selection__rendered {
    width: 100%;
  }
  .ant-select-selection__placeholder {
    display: block;
  }
  .ant-select-selection-selected-value {
    font-weight: bold;
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
    placeholder,
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
        placeholder={placeholder}
      >
        {lista.length &&
          lista.map(item => {
            return (
              <Option key={shortid.generate()} value={`${item[valueOption]}`}>
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
  label: PropTypes.string.isRequired,
  valueOption: PropTypes.string.isRequired,
  valueSelect: PropTypes.string,
  lista: PropTypes.array,
  placeholder: PropTypes.string,
};

export default SelectComponent;
