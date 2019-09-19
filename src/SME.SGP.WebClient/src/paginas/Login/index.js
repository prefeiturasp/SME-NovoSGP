import React from 'react';

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
