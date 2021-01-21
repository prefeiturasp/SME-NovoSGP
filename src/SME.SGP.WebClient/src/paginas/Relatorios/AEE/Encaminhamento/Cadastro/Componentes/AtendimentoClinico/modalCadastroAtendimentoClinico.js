import PropTypes from 'prop-types';
import React, { useState } from 'react';
import shortid from 'shortid';
import {
  CampoData,
  CampoTexto,
  Colors,
  ModalConteudoHtml,
  SelectComponent,
} from '~/componentes';
import Button from '~/componentes/button';

const ModalCadastroAtendimentoClinico = props => {
  const { onClose, exibirModal } = props;

  const [diaSemana, setDiaSemana] = useState();
  const [atendimentoAtividade, setAtendimentoAtividade] = useState();
  const [localRealizacao, setLocalRealizacao] = useState();
  const [horarioInicio, setHorarioInicio] = useState();
  const [horarioTermino, setHorarioTermino] = useState();

  const listaDiasSemana = [
    {
      valor: 'Domingo',
      desc: 'Domingo',
    },
    {
      valor: 'Segunda',
      desc: 'Segunda',
    },
    {
      valor: 'Terça',
      desc: 'Terça',
    },
    {
      valor: 'Quarta',
      desc: 'Quarta',
    },
    {
      valor: 'Quinta',
      desc: 'Quinta',
    },
    {
      valor: 'Sexta',
      desc: 'Sexta',
    },
    {
      valor: 'Sábado',
      desc: 'Sábado',
    },
  ];

  const onChangeDiaSemana = valor => {
    setDiaSemana(valor);
  };

  const onChangeAtendimentoAtividade = e => {
    setAtendimentoAtividade(e.target.value);
  };

  const onChangeLocalRealizacao = e => {
    setLocalRealizacao(e.target.value);
  };

  const onChangeHorarioInicio = valor => {
    setHorarioInicio(valor);
  };

  const onChangeHorarioTermino = valor => {
    setHorarioTermino(valor);
  };

  const limparDadosModal = () => {
    setDiaSemana();
    setAtendimentoAtividade();
    setLocalRealizacao();
    setHorarioInicio();
    setHorarioTermino();
  };

  const fecharModal = () => {
    limparDadosModal();
    onClose();
  };

  const onSalvar = () => {
    const params = {};

    if (diaSemana) {
      params.diaSemana = diaSemana;
    }
    if (atendimentoAtividade) {
      params.atendimentoAtividade = atendimentoAtividade;
    }
    if (localRealizacao) {
      params.localRealizacao = localRealizacao;
    }
    if (horarioInicio) {
      params.horarioInicio = horarioInicio;
    }
    if (horarioTermino) {
      params.horarioTermino = horarioTermino;
    }

    limparDadosModal();
    onClose(params);
  };

  return (
    <ModalConteudoHtml
      id={shortid.generate()}
      key="detalhamento-atendimento-clinico"
      visivel={exibirModal}
      titulo="Detalhamento de atendimento clínico"
      onClose={fecharModal}
      esconderBotaoPrincipal
      esconderBotaoSecundario
      width={750}
      closable
    >
      <div className="col-md-12 mb-2">
        <SelectComponent
          label="Dias da Semana"
          lista={listaDiasSemana}
          valueOption="valor"
          valueText="desc"
          valueSelect={diaSemana}
          onChange={onChangeDiaSemana}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoTexto
          onChange={onChangeAtendimentoAtividade}
          label="Atendimento/Atividade"
          value={atendimentoAtividade}
          maxLength={100}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoTexto
          onChange={onChangeLocalRealizacao}
          label="Local de realização"
          value={localRealizacao}
          maxLength={100}
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoData
          onChange={onChangeHorarioInicio}
          label="Horário de início"
          valor={horarioInicio}
          placeholder="09:00"
          formatoData="HH:mm"
          somenteHora
        />
      </div>
      <div className="col-md-12 mb-2">
        <CampoData
          onChange={onChangeHorarioTermino}
          valor={horarioTermino}
          label="Horário término"
          placeholder="09:30"
          formatoData="HH:mm"
          somenteHora
        />
      </div>

      <div className="col-md-12 mt-2 p-0 d-flex justify-content-end">
        <Button
          key="btn-voltar"
          id="btn-voltar"
          label="Cancelar"
          color={Colors.Azul}
          border
          onClick={fecharModal}
          className="mt-2 mr-2"
        />
        <Button
          key="btn-salvar"
          id="btn-salvar"
          label="Adicionar Registro"
          color={Colors.Roxo}
          border
          onClick={onSalvar}
          className="mt-2"
        />
      </div>
    </ModalConteudoHtml>
  );
};

ModalCadastroAtendimentoClinico.propTypes = {
  onClose: PropTypes.func,
  exibirModal: PropTypes.bool,
};

ModalCadastroAtendimentoClinico.defaultProps = {
  onClose: () => {},
  exibirModal: false,
};

export default ModalCadastroAtendimentoClinico;
