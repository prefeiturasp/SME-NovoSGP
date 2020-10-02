import styled from 'styled-components';

export const EstiloDetalhe = styled.div`
  width: 100%;
  border: 1px solid #dadada;
  border-radius: 4px;
  margin: 0px 15px 27px 15px;
  padding: 20px;

  .bg-id {
    background-color: #d9d8d8;
    border-radius: 4px;
    font-size: 14px;
    height: 72px;
    font-weight: bold;
    justify-content: center;
    justify-items: center;
    display: flex;
    padding: 10px;

    .id-notificacao {
      font-size: 20px;
    }
  }

  .notificacao-horario {
    font-size: 12px;
    color: #a8a8a8;
  }

  div.titulo-coluna {
    padding-top: 25px;
    font-size: 14px;
    font-weight: bold;
    color: #42474a;

    div.conteudo-coluna {
      font-size: 14px;
      color: #42474a;
      font-weight: normal;
    }
  }

  .mg-bottom {
    margin-bottom: solid 1px #c0c0c0;
  }

  .mensagem {
    font-size: 14px;
    line-height: 1.57;
    letter-spacing: normal;
    color: #42474a;
    margin-top: 39px;
  }

  .mt-hr {
    margin-top: 34px;
  }

  div.obs {
    margin-top: 40px;

    label {
      font-size: 14px;
      font-weight: bold;
      color: #42474a;
    }
    textarea {
      font-size: 12px;
      line-height: 1.6;
      letter-spacing: normal;
    }
  }

  .btn-baixar-relatorio {
    font-stretch: normal;
    height: 100%;
    letter-spacing: normal;
    line-height: normal;
    font-family: 'FontAwesome', 'Roboto', sans-serif !important;
    color: white;
    background: #064f79;
    border-radius: 2.52361px;
    padding: 15px;

    :hover {
      color: white !important;
    }
  }
`;
