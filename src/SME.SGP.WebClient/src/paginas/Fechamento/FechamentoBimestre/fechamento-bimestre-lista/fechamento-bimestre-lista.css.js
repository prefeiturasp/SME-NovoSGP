import styled from 'styled-components';
import { Base } from '~/componentes';

export const TabelaFechamento = styled.div`
  .fundo-cinza {
    background-color: ${Base.CinzaFundo};
    vertical-align: middle !important;
  }

  .container-table {
    width: 100%;
    overflow: auto;
  }

  th,
  td {
    border: solid 1px ${Base.CinzaDesabilitado};
  }

  .tabela-fechamento-thead {
    background: ${Base.CinzaFundo} !important;
    text-align: center;
    border-left: solid 1px ${Base.CinzaDesabilitado};

    th {
      border-right: solid 1px ${Base.CinzaDesabilitado};
      border-bottom: 1px solid ${Base.CinzaDesabilitado};
    }
  }
`;

export const Marcadores = styled.div`
  vertical-align: bottom;
`;

export const MarcadorAulas = styled.div`
  background-color: ${Base.Roxo};
  color: ${Base.Branco};
  height: 22px;
  min-width: 123px;
  display: 'flex';
  align-items: 'center';
  justify-content: 'center';
  font-size: 12px;
  text-align: center;
  border-radius: 4px;
  margin-top: 15px;

  .numero {
    font-weight: bold;
  }
`;

export const Info = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 10px;
  margin-left: 2px;
  position: absolute;
  padding-top: 5px;
`;

export const MaisMenos = styled.i`
  color: ${Base.Roxo} !important;
  font-size: 18px;
`;

export const SituacaoProcessadoComPendencias = styled.div`
  background-color: ${Base.Roxo};
  color: ${Base.Branco};
  height: 22px;
  font-size: 12px;
  border-radius: 4px;
  margin-top: 15px;
  span {
    margin: 10px;
  }
`;

export const  DataFechamentoProcessado = styled.div`  
  font-size: 12px;
  font-weight: bold;  
  color: rgb(33, 37, 41);
`;
