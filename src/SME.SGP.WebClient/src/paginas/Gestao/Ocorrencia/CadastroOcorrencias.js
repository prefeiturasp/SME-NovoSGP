import React from 'react';
import shortid from 'shortid';
import { Button, Card, Colors } from '~/componentes';
import { Cabecalho } from '~/componentes-sgp';

const CadastroOcorrencias = () => {
  const onClickVoltar = () => {};
  const onClickCancelar = () => {};
  const onClickCadastrar = () => {};

  const criancas = [
    { nome: 'Ana', rf: '123456' },
    { nome: 'Julio', rf: '123456' },
    { nome: 'Pedro', rf: '123456' },
  ];

  return (
    <>
      <Cabecalho pagina="Cadastro de ocorrência" />
      <Card>
        <div className="col-md-12 d-flex justify-content-end pb-4">
          <Button
            id={shortid.generate()}
            label="Voltar"
            icon="arrow-left"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickVoltar}
          />
          <Button
            id={shortid.generate()}
            label="Cancelar"
            color={Colors.Azul}
            border
            className="mr-2"
            onClick={onClickCancelar}
          />
          <Button
            id={shortid.generate()}
            label="Cadastrar"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickCadastrar}
          />
        </div>
        <div className="col-12 mb-3 font-weight-bold">
          <span>Crianças envolvidas na ocorrência</span>
        </div>
        <div className="col-12">
          {criancas.map(crianca => {
            return (
              <div className="mb-3">
                <span>
                  {crianca.nome} ({crianca.rf})
                </span>
                <br />
              </div>
            );
          })}
        </div>
      </Card>
    </>
  );
};

export default CadastroOcorrencias;
