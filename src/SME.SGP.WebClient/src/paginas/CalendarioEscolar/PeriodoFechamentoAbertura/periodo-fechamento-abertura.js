import React, { useState } from 'react';

const PeriodoFechamentoAbertura = () => {
  const chaveBimestre = {
    primeiroInicio: 'primeiroBimestreDataInicial',
    primeiroFinal: 'primeiroBimestreDataFinal',
    segundoInicio: 'segundoBimestreDataInicial',
    segundoFinal: 'segundoBimestreDataFinal',
    terceiroInicio: 'terceiroBimestreDataInicial',
    terceiroFinal: 'terceiroBimestreDataFinal',
    quartoInicio: 'quartoBimestreDataInicial',
    quartoFinal: 'quartoBimestreDataFinal',
  };

  const onChangeCamposData = () => {};

  const criaBimestre = (form, descricao, chaveDataInicial, chaveDataFinal) => {
    return (
      <div className="row">
        <div className="col-sm-4 col-md-4 col-lg-2 col-xl-2 mb-2">
          <CaixaBimestre>
            <BoxTextoBimetre>{descricao}</BoxTextoBimetre>
          </CaixaBimestre>
        </div>
        <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
          <CampoData
            form={form}
            placeholder="Início do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataInicial}
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
        </div>
        <div className="col-sm-4 col-md-4 col-lg-3 col-xl-3">
          <CampoData
            form={form}
            placeholder="Fim do Bimestre"
            formatoData="DD/MM/YYYY"
            name={chaveDataFinal}
            onChange={onChangeCamposData}
            desabilitado={desabilitaCampos}
          />
        </div>
      </div>
    );
  };
  return <></>;
};

export default PeriodoFechamentoAbertura;
