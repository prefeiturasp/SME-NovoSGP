import styled from 'styled-components';
import Grid from '~/componentes/grid';

const Botoes = styled(Grid)`
  align-items: center;
  position: relative;
  width: 100%;

  .btn.attached.left {
    border-top-right-radius: 0;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
    position: relative;
  }

  .btn.attached.right {
    border-top-left-radius: 0;
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
    position: relative;
  }
`;

const Container = styled.div`
  .ant-card-head {
    min-height: auto;
    padding: 0 0 0 24px;

    .ant-card-head-title {
      padding: 0;
    }
  }
  .anticon {
    vertical-align: middle;
  }
  .fa {
    margin: 0 !important;
  }
`;

export { Botoes, Container };
