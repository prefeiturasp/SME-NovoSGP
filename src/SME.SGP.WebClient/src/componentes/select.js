import React from 'react';
import PropTypes from 'prop-types';
import styled from 'styled-components';
import { Select } from 'antd';
import Icon from 'antd/es/icon';
import shortid from 'shortid';
import { Field } from 'formik';
import { Base } from './colors';
import Label from './label';

const SelectComponent = React.forwardRef((props, ref) => {
  const {
    name,
    id,
    className,
    classNameContainer,
    onChange,
    label,
    valueText,
    valueOption,
    valueSelect,
    lista,
    placeholder,
    alt,
    multiple,
    containerVinculoId,
    disabled,
    form,
    showSearch,
    size,
    border,
    color,
    allowClear,
    defaultValue,
  } = props;

  const Container = styled.div`
    ${size && size === 'small' && 'height: 24px;'}

    .ant-select {
      width: 100%;
    }

    .ant-select-arrow {
      color: ${Base.CinzaMako};
    }

    .ant-select-selection {
      ${border && `border-color: ${border};`}
    }

    .ant-select-selection__placeholder {
      ${color && `color: ${color};`}
      ${border && color && 'font-weight: bold !important;'}
    }

    .ant-select-selection--single {
      align-items: center;
      display: flex;
      ${!size && 'height: 38px;'}
    }

    .ant-select-selection__rendered {
      width: 98%;
    }

    .ant-select-selection__placeholder {
      display: block;
    }

    .ant-select .ant-select-search__field {
      display: block;
      max-width: 100% !important;
    }

    .ant-select-selection-selected-value {
      font-weight: bold;
    }

    .ant-select-selection--multiple {
      min-height: 38px;

      .ant-select-selection__placeholder {
        line-height: 25px;
      }

      .ant-select-selection__rendered {
        margin-top: 3px;
      }
    }

    div[class*='is-invalid'] {
      .ant-select-selection {
        border-color: #dc3545 !important;
      }
    }

    label {
      font-weight: bold;
    }
  `;

  const Erro = styled.span`
    color: ${Base.Vermelho};
  `;

  const { Option } = Select;

  const possuiErro = () => {
    return form && form.errors[name] && form.touched[name];
  };

  const opcoesLista = () => {
    return (
      lista &&
      lista.length > 0 &&
      lista.map(item => {
        return (
          <Option
            key={shortid.generate()}
            value={`${item[valueOption]}`}
            title={`${item[valueText]}`}
          >
            {`${item[valueText]}`}
          </Option>
        );
      })
    );
  };

  const obterErros = () => {
    return form && form.touched[name] && form.errors[name] ? (
      <Erro>{form.errors[name]}</Erro>
    ) : (
      ''
    );
  };

  const campoComValidacoes = () => (
    <Field
      mode={multiple && 'multiple'}
      suffixIcon={<Icon type="caret-down" />}
      className={
        form
          ? `overflow-hidden ${possuiErro() ? 'is-invalid' : ''} ${className ||
              ''}`
          : ''
      }
      name={name}
      id={id || name}
      value={form.values[name] || undefined}
      placeholder={placeholder}
      notFoundContent="Sem dados"
      alt={alt}
      optionFilterProp="children"
      allowClear={allowClear}
      disabled={disabled}
      component={Select}
      type="input"
      onChange={e => {
        form.setFieldValue(name, e || '');
        form.setFieldTouched(name, true, true);
        if (onChange) onChange(e || '');
      }}
      innerRef={ref}
      defaultValue={defaultValue}
    >
      {opcoesLista()}
    </Field>
  );

  const obtenhaContainerVinculo = () =>
    document.getElementById(containerVinculoId);

  const campoSemValidacoes = () => (
    <Select
      mode={multiple && 'multiple'}
      suffixIcon={<Icon type="caret-down" />}
      className={`overflow-hidden ${className}`}
      name={name}
      id={id}
      onChange={onChange}
      value={valueSelect}
      getPopupContainer={containerVinculoId && obtenhaContainerVinculo}
      placeholder={placeholder}
      notFoundContent="Sem dados"
      alt={alt}
      optionFilterProp="children"
      allowClear={allowClear}
      disabled={disabled}
      ref={ref}
      showSearch={showSearch}
      size={size || 'default'}
      defaultValue={defaultValue}
    >
      {opcoesLista()}
    </Select>
  );
  return (
    <Container className={classNameContainer && classNameContainer}>
      {label ? <Label text={label} control={name} /> : ''}
      {form ? campoComValidacoes() : campoSemValidacoes()}
      {form ? obterErros() : ''}
    </Container>
  );
});

SelectComponent.propTypes = {
  name: PropTypes.string,
  id: PropTypes.string,
  className: PropTypes.string,
  classNameContainer: PropTypes.string,
  onChange: PropTypes.func,
  label: PropTypes.string,
  valueText: PropTypes.string.isRequired,
  valueOption: PropTypes.string.isRequired,
  valueSelect: PropTypes.oneOfType([PropTypes.any]),
  lista: PropTypes.array.isRequired,
  placeholder: PropTypes.string,
  alt: PropTypes.string,
  multiple: PropTypes.bool,
  containerVinculoId: PropTypes.string,
  disabled: PropTypes.bool,
  form: PropTypes.any,
  showSearch: PropTypes.bool,
  size: PropTypes.string,
  border: PropTypes.string,
  color: PropTypes.string,
  allowClear: PropTypes.bool,
};

SelectComponent.defaultProps = {
  allowClear: true,
};

export default SelectComponent;
