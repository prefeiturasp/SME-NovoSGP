import styled from 'styled-components';

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

  .display-block {
    display: block !important;
  }
`;

const DadosAluno = styled.div`
  width: 100%;
  height: 100%;
  color: #42474a;
  display: flex;
  align-items: center;

  p {
    margin-bottom: 0;
  }
`;

const FrequenciaGlobal = styled.div`
  font-weight: 700 !important;
  text-align: end;
  font-size: 12px !important;
`;

export { Container, DadosAluno, FrequenciaGlobal };
