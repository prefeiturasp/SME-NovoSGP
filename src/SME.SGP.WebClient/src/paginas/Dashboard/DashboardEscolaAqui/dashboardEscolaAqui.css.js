import styled from 'styled-components';
import { Base } from '~/componentes/colors';

export const ContainerTabsDashboardEscolaAqui = styled.div`
  .ant-tabs-nav {
    width: 33.33% !important;
  }

  .scrolling-chart {
    display: flex;
    flex-wrap: nowrap;
    overflow-x: auto;

    ::-webkit-scrollbar-track {
      background-color: #f4f4f4 !important;
    }

    ::-webkit-scrollbar {
      width: 4px !important;
      background-color: rgba(229, 237, 244, 0.71) !important;
      border-radius: 2.5px !important;
    }

    ::-webkit-scrollbar-thumb {
      background: #a8a8a8 !important;
      border-radius: 3px !important;
    }
  }
`;

export const ContainerDataUltimaAtualizacao = styled.span`
  background-color: ${Base.Roxo};
  border: solid 0.5px ${Base.Roxo};
  border-radius: 3px;
  color: ${Base.Branco};
  font-weight: bold;
  padding: 0px 5px 0px 5px;
`;

export const MarcadorSituacaoAluno = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 10px;
  margin-left: 2px;
  padding-top: 5px;
`;
