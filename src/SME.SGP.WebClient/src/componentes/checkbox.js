import { Checkbox } from 'antd';
import PropTypes from 'prop-types';
import React from 'react';
import styled from 'styled-components';

import { Base } from './colors';

const Container = styled.div`
  .ant-checkbox-checked .ant-checkbox-inner {
    background-color: ${Base.Roxo};
    border-color: ${Base.Roxo};
  }
  .ant-checkbox-wrapper:hover .ant-checkbox-inner,
  .ant-checkbox:hover .ant-checkbox-inner,
  .ant-checkbox-input:focus + .ant-checkbox-inner {
    border-color: ${Base.Roxo};
  }
`;

const CheckboxComponent = props => {
  const { label, onChangeCheckbox, defaultChecked, className, disabled, checked } = props;

  return (
    <Container className={className}>
      <Checkbox onChange={onChangeCheckbox} defaultChecked={defaultChecked} disabled={disabled} checked={checked}>
        {label}
      </Checkbox>
    </Container>
  );
};

CheckboxComponent.propTypes = {
  label: PropTypes.string,
  defaultChecked: PropTypes.bool,
  className: PropTypes.string,
  disabled: PropTypes.bool,
  checked: PropTypes.bool.isRequired,
};

CheckboxComponent.defaultProps = {
  label: 'Checkbox Component',
  defaultChecked: false,
  className: '',
  disabled: false,
  checked: false
};

export default CheckboxComponent;
