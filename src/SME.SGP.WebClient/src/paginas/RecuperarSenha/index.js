import React, { useLayoutEffect, useState, useRef } from 'react';
import styled from 'styled-components';
import LogoDoSgp from '~/recursos/LogoDoSgp.svg';
import { Base, Colors } from '~/componentes/colors';
import Button from '~/componentes/button';
import history from '~/servicos/history';
import Sucesso from './sucesso';
import Erro from './erro';
import Orientacoes from './orientacoes';
import api from '~/servicos/api';
import { Loader } from '~/componentes';

const Nav = styled.nav`
  height: 70px;
`;

const Logo = styled.img`
  max-height: 65px;
  max-width: 75px;
`;

const Div = styled.div`
  button {
    margin-right: 1rem;
  }
  button:last-child {
    margin-right: 0;
  }
`;

const Container = styled(Div)`
  background: ${Base.Branco};
  height: 100%;
`;

const Titulo = styled.h1`
  font-size: 24px;
  font-weight: bold;
`;

const Texto = styled(Div)`
  font-size: 14px;
  letter-spacing: normal;
  line-height: normal;
`;

const Input = styled.input`
  border: 1px solid ${Base.CinzaDesabilitado};
  color: ${Base.CinzaBotao};
  font-size: 14px;
  &[placeholder] {
    color: ${Base.CinzaBotao};
  }
`;

const RecuperarSenha = props => {
  const [rf, setRf] = useState();
  const [retorno, setRetorno] = useState(false);
  const [mensagem, setMensagem] = useState('');
  const [email, setEmail] = useState('');
  const [carregando, setCarregando] = useState(false);
  const refInput = useRef();

  useLayoutEffect(() => {
    if (props.location && props.location.state) {
      if (props.location.state.rf) {
        setRf(props.location.state.rf);
      }
    }
  }, []);

  useLayoutEffect(() => {
    refInput.current.focus();
  }, [rf]);

  const consultaAPI = async () => {
    setCarregando(true);
    api
      .post(`/v1/autenticacao/solicitar-recuperacao-senha/?login=${rf}`)
      .then(resposta => {
        setEmail(resposta.data);
        setRetorno({ status: true });
      })
      .catch(erro => {
        setMensagem(erro.response.data.mensagens[0]);
        setRetorno({ status: false });
      })
      .finally(() => {
        //setCarregando(false);
      });
  };

  const onChangeUsuario = () => {
    setRf(refInput.current.value);
  };

  const onClickVoltar = () => {
    history.push('/login');
  };

  const onClickCancelar = () => {
    setRf('');
  };

  const onClickContinuar = () => {
    consultaAPI();
  };

  const renderizaConteudo = () => {
    if (typeof retorno === 'object') {
      if (retorno.status) return <Sucesso email={email} />;
      return <Erro mensagem={mensagem} />;
    }
    return <Orientacoes />;
  };

  const renderizaBotoes = () => {
    if (typeof retorno === 'object')
      return (
        <Button
          label="Continuar"
          color={Colors.Roxo}
          className="btn-block w-75"
          onClick={onClickVoltar}
          bold
        />
      );
    return (
      <>
        <Button
          label="Voltar"
          color={Colors.Azul}
          border
          icon="arrow-left"
          onClick={onClickVoltar}
          bold
        />
        <Button
          label="Cancelar"
          color={Colors.Roxo}
          border
          onClick={onClickCancelar}
          bold
        />
        <>
          <Loader loading={carregando} ignorarTip>
            <Button
              label="Continuar"
              color={Colors.Roxo}
              disabled={!rf}
              border
              onClick={onClickContinuar}
              bold
            />
          </Loader>
        </>
      </>
    );
  };

  return (
    <>
      <Nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top py-0">
        <Logo
          src={LogoDoSgp}
          className="mx-auto"
          alt="Novo Sistema de Gestão Pedagógica"
        />
      </Nav>
      <Container className="container text-center shadow-sm mx-auto mt-4 rounded">
        <Div className="row">
          <Div className="col-xl-12 col-lg-12 col-md-12 col-sm-12">
            <Texto
              className="mx-auto"
              style={{ marginBottom: '70px', maxWidth: '560px' }}
            >
              <Titulo style={{ marginTop: '70px', marginBottom: '40px' }}>
                Recuperação de Senha
              </Titulo>
              {renderizaConteudo()}
            </Texto>
            {!retorno && (
              <Div
                className="mx-auto text-left"
                style={{ marginBottom: '70px', maxWidth: '560px' }}
              >
                <form className="w-100">
                  <div className="form-group">
                    <label htmlFor="usuario">Usuário</label>
                    <Input
                      type="text"
                      id="usuario"
                      className="form-control form-control-lg rounded"
                      placeholder="Insira seu usuário ou RF"
                      value={rf}
                      onChange={onChangeUsuario}
                      ref={refInput}
                    />
                  </div>
                </form>
              </Div>
            )}
            <Div
              className={`mx-auto d-flex ${
                !retorno ? 'justify-content-end' : 'justify-content-center'
              }`}
              style={{ marginBottom: '70px', maxWidth: '560px' }}
            >
              {renderizaBotoes()}
            </Div>
          </Div>
        </Div>
      </Container>
    </>
  );
};

export default RecuperarSenha;
