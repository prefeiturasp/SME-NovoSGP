import styled from 'styled-components';

export const ContainerLegendaGrafico = styled.div`
  margin-left: 70px;
  width: 70vw;
  font-size: 12px;

  .legenda-centralizada {
    display: flex;
    justify-content: center;
  }

  .legenda-container-conteudo {
    display: flex;
    align-items: center;
    margin: 5px;
    text-align: initial;

    .cor-legenda {
      display: flex;
      align-items: center;
    }

    .label-valor {
      color: #000000;
      font-weight: bold;
    }

    span {
      display: block;
      height: 16px;
      width: 16px;
      margin-right: 10px;
      border-radius: 4px;
    }

    .label-valor-porcentagem {
      color: gray;
      font-style: italic;
    }
  }
`;
