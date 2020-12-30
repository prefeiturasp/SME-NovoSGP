import React, { useState } from 'react';
import shortid from 'shortid';
import { Cabecalho } from '~/componentes-sgp';
import {
  Card,
  Button,
  Colors,
  CampoData,
  LocalizadorCrianca,
} from '~/componentes';

const ListaOcorrencias = () => {
  const [dataInicial, setDataInicial] = useState();
  const [dataFinal, setDataFinal] = useState();

  const onClickVoltar = () => {};
  const onClickExcluir = () => {};
  const onClickNovo = () => {};

  const onChangeDataInicial = () => {};
  const onChangeDataFinal = () => {};
  const onChangeLocalizadorEstudante = () => {};

  return (
    <>
      <Cabecalho pagina="Ocorrências" />
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
            label="Excluir"
            color={Colors.Vermelho}
            border
            className="mr-2"
            onClick={onClickExcluir}
          />
          <Button
            id={shortid.generate()}
            label="Nova"
            color={Colors.Roxo}
            border
            bold
            className="mr-2"
            onClick={onClickNovo}
          />
        </div>
        <div className="col-sm-12 col-md-3">
          <CampoData
            label="Data da ocorrência"
            valor={dataInicial}
            onChange={onChangeDataInicial}
            placeholder="Data inicial"
            formatoData="DD/MM/YYYY"
          />
        </div>
        <div className="col-sm-12 col-md-3" style={{ marginTop: '25px' }}>
          <CampoData
            valor={dataInicial}
            onChange={onChangeDataFinal}
            placeholder="Data final"
            formatoData="DD/MM/YYYY"
          />
        </div>
        <div className="col-sm-12 col-md-6">
          <LocalizadorCrianca
            label="Criança"
            placeHolder="Procure pelo nome da criança"
          />
        </div>
      </Card>
    </>
  );
};

export default ListaOcorrencias;
