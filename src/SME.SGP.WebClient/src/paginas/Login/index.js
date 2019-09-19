import React from 'react';
import { Row } from 'antd';
import styled from 'styled-components';
import FundoLogin from '~/recursos/FundoLogin.svg';
import LogoSgp from '~/recursos/LogoSgpTexto.svg';
import Card from '~/componentes/cardBootstrap';
import Grid from '~/componentes/grid';
import { Base } from '~/componentes/colors';
import CardBody from '~/componentes/cardBody';
import CampoTexto from '~/componentes/campoTexto';
import FormGroup from '~/componentes/formGroup';
import Button from '~/componentes/button';

const Fundo = styled(Row)`
  background: url(${FundoLogin});
  height: 100vh;
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
`;

const Logo = styled.img`
  max-height: 131px !important;
  max-width: 339px !important;
`;

const Cartao = styled(Card)`
  display: flex;
  height: 100vh;
  border-radius: 6px;
`;
const Centralizar = styled.div`
  display: flex;
  justify-content: center;
`;

export default function Login() {
  return (
    <Fundo>
      <Grid cols={12} className="d-flex justify-content-end p-0">
        <Cartao className="col-md-6">
          <CardBody>
            <Grid cols={12}>
              <Logo
                src={LogoSgp}
                alt="SGP"
                className="mx-auto d-block col-md-6"
              />
            </Grid>
            <Centralizar>
              <Grid cols={6} className="p-0">
                <FormGroup className="col-md-12 p-0">
                  <label htmlFor="Usuario">Usuário</label>
                  <input
                    id="Usuario"
                    placeholder="Insira seu usuário ou RF"
                    className="col-md-12 form-control"
                  />
                </FormGroup>
                <FormGroup className="col-md-12 p-0">
                  <label htmlFor="Senha">Senha</label>
                  <input
                    id="Senha"
                    placeholder="Insira sua senha"
                    className="col-md-12 form-control"
                  />
                </FormGroup>
                <FormGroup>
                  <Button style="primary" label="Acessar" color={Base.Roxo} />
                </FormGroup>
              </Grid>
            </Centralizar>
          </CardBody>
        </Cartao>
      </Grid>
    </Fundo>
  );
}
