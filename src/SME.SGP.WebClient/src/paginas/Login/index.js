import React from 'react';
import Row from '~/componentes/row';
import styled from 'styled-components';
import FundoLogin from '~/recursos/FundoLogin.svg';
import LogoDoSgp from '~/recursos/LogoSgpTexto.svg';
import LogoCidadeSP from '~/recursos/LogoCidadeSP.svg';
import Card from '~/componentes/cardBootstrap';
import Grid from '~/componentes/grid';
import { Base, Colors } from '~/componentes/colors';
import CardBody from '~/componentes/cardBody';
import FormGroup from '~/componentes/formGroup';
import Button from '~/componentes/button';
import { Tooltip } from 'antd';
import Icon from '~/componentes/icon';
import LinkRouter from '~/componentes/linkRouter';

const Fundo = styled(Row)`
  background: url(${FundoLogin});
  height: 100%;
  height: -moz-available;
  height: -webkit-fill-available;
  height: fill-available;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
`;

const Logo = styled.img`
  align-self: center;
  justify-self: center;
`;

const Formulario = styled.form`
  padding: 0px;

  align-self: center;
  justify-self: center;
`;

const LogoSGP = styled.div`
  padding: 0px;
`;

const CampoTexto = styled.input`
  ::-webkit-input-placeholder {
    font-size: 14px !important;
    font-family: Roboto !important;
    font-size: 14px !important;
    font-weight: normal !important;
    font-style: normal !important;
    font-stretch: normal !important;
    line-height: 1.6 !important;
    letter-spacing: normal !important;
    color: ${Base.CinzaBotao} !important;
  }
  ::-moz-placeholder {
    font-size: 14px !import;
  }
  :-ms-input-placeholder {
    font-size: 14px !import;
  }
  :-moz-placeholder {
    font-size: 14px !import;
  }
`;

const Rotulo = styled.label`
  font-family: Roboto !important;
  font-size: 14px !important;
  font-weight: normal !important;
  font-style: normal !important;
  font-stretch: normal !important;
  line-height: normal !important;
  letter-spacing: normal !important;
  color: ${Base.CinzaMako} !important;
`;

const LogoSP = styled(LogoSGP)`
  align-self: end;
  justify-self: center;
  justify-content: center;
  justify-items: center;
`;

const Cartao = styled(Card)`
  height: auto;
  border-radius: 6px;
`;

const CorpoCartao = styled(CardBody)`
  display: flex;
  height: 100%;
  justify-content: center;
`;

const Centralizar = styled.div`
  display: flex;
  justify-content: center;
`;

const Link = styled(LinkRouter)`
  justify-self: center;
  justify-content: center;
  justify-items: center;
`;

const LabelLink = styled.label`
  font-family: Roboto;
  font-size: 12px;
  font-weight: normal;
  font-style: normal;
  font-stretch: normal;
  color: ${Base.Roxo} !important;
`;

const TextoAjuda =
  'Para usuários externos, insira seus dados de usuário. Caso não possua, procure a SME.';

export default function Login() {
  return (
    <Fundo className="p-0">
      <Grid cols={12} className="d-flex justify-content-end">
        <Cartao className="col-xl-6 col-lg-6 col-md-8 col-sm-8 col-xs-12">
          <CorpoCartao className="">
            <Centralizar className="row col-md-12">
              <Row className="col-md-12 p-0 d-flex justify-content-center align-self-start">
                <LogoSGP className="col-xl-8 col-md-8 col-sm-8 col-xs-12">
                  <Logo src={LogoDoSgp} alt="SGP" />
                </LogoSGP>
              </Row>
              <Row className="col-md-12 d-flex justify-content-center align-self-start">
                <Formulario
                  id="Formulario"
                  className="col-xl-8 col-md-8 col-sm-8 col-xs-12 p-0"
                >
                  <FormGroup className="col-md-12 p-0">
                    <Rotulo className="d-block" htmlFor="Usuario">
                      Usuário
                    </Rotulo>
                    <Centralizar class="col-md-12">
                      <CampoTexto
                        id="Usuario"
                        placeholder="Insira seu usuário ou RF"
                        className="col-md-12 form-control"
                      />
                    </Centralizar>
                  </FormGroup>
                  <FormGroup className="col-md-12 p-0">
                    <Rotulo htmlFor="Senha">Senha</Rotulo>
                    <CampoTexto
                      id="Senha"
                      placeholder="Insira sua senha"
                      className="col-md-12 form-control"
                    />
                  </FormGroup>
                  <FormGroup>
                    <Button
                      style="primary"
                      className="btn-block d-block"
                      label="Acessar"
                      color={Colors.Roxo}
                    />
                    <Centralizar className="mt-1">
                      <Link to="/" isactive>
                        <LabelLink>Esqueci minha senha</LabelLink>
                      </Link>
                    </Centralizar>
                  </FormGroup>
                </Formulario>
              </Row>
              <Row className="col-md-12 d-flex justify-content-center align-self-end mb-5">
                <LogoSP className="col-xl-8 col-md-8 col-sm-8 col-xs-12 d-flex">
                  <Logo src={LogoCidadeSP} alt="Cidade de São Paulo" />
                </LogoSP>
              </Row>
            </Centralizar>
          </CorpoCartao>
        </Cartao>
      </Grid>
    </Fundo>
  );
}
