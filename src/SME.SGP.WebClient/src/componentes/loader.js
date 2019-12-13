import React from 'react';
import PropTypes from 'prop-types';

// Ant
import { Spin, Icon } from 'antd';

// Styles
import styled from 'styled-components';

// Componentes
import { Base } from '~/componentes';

const LoaderWrapper = styled.div`
  .ant-spin-text {
    color: ${Base.CinzaMako};
  }
`;

const icone = <Icon type="loading" style={{ fontWeight: 'bold' }} spin />;

function Loader({ children, loading, tip }) {
  return (
    <LoaderWrapper>
      <Spin tip={tip} size="large" indicator={icone} spinning={loading}>
        {children}
      </Spin>
    </LoaderWrapper>
  );
}

Loader.propTypes = {
  children: PropTypes.oneOfType([
    PropTypes.node,
    PropTypes.any,
    PropTypes.symbol,
  ]),
  loading: PropTypes.bool,
  tip: PropTypes.string,
};

Loader.defaultProps = {
  loading: false,
  children: () => {},
  tip: 'Carregando...',
};

export default Loader;
