import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';

// Redux
import { useSelector } from 'react-redux';

// Componentes
import InputRF from './componentes/InputRF';
import InputNome from './componentes/InputNome';
import { Grid, Label } from '~/componentes';

// Services
import service from './services/LocalizadorService';
import { erro, erros } from '~/servicos/alertas';

// Funções
import { validaSeObjetoEhNuloOuVazio } from '~/utils/funcoes/gerais';

// Utils
import RFNaoEncontradoExcecao from '~/utils/excecoes/RFNãoEncontradoExcecao';

function Localizador({
  onChange,
  showLabel,
  form,
  dreId,
  anoLetivo,
  desabilitado,
  rfEdicao,
  buscarOutrosCargos,
  buscandoDados,
  labelRF,
  placeholderRF,
  placeholderNome,
  labelNome,
  classesRF,
  limparCamposAposPesquisa,
  validaPerfilProfessor,
  mensagemErroConsultaRF,
}) {
  const usuario = useSelector(store => store.usuario);
  const [dataSource, setDataSource] = useState([]);
  const [pessoaSelecionada, setPessoaSelecionada] = useState({});
  const [desabilitarCampo, setDesabilitarCampo] = useState({
    rf: false,
    nome: false,
  });
  const { ehPerfilProfessor, rf } = usuario;
  const [exibirLoader, setExibirLoader] = useState(false);

  const validacaoDesabilitaPerfilProfessor = () => {
    return validaPerfilProfessor && ehPerfilProfessor;
  };

  const onChangeInput = async valor => {
    if (valor.length === 0) {
      setPessoaSelecionada({
        professorRf: '',
        professorNome: '',
        usuarioId: '',
      });
      setTimeout(() => {
        setDesabilitarCampo(() => ({
          rf: false,
          nome: false,
        }));
      }, 200);
    }

    if (valor.length < 3) return;
    setExibirLoader(true);
    const { data: dados } = await service
      .buscarAutocomplete({
        nome: valor,
        dreId,
        anoLetivo,
      })
      .finally(() => setExibirLoader(false));

    if (dados && dados.length > 0) {
      setDataSource(
        dados.map(x => ({
          professorRf: x.codigoRF,
          professorNome: x.nome,
          usuarioId: x.usuarioId,
        }))
      );
    }
  };

  const onBuscarPorRF = useCallback(
    async ({ rf }) => {
      try {
        buscandoDados(true);
        setExibirLoader(true);
        const { data: dados } = await service
          .buscarPorRf({
            rf,
            anoLetivo,
            buscarOutrosCargos,
          })
          .finally(() => setExibirLoader(false));

        if (!dados) throw new RFNaoEncontradoExcecao();

        setPessoaSelecionada({
          professorRf: dados.codigoRF,
          professorNome: dados.nome,
          usuarioId: dados.usuarioId,
        });

        setDesabilitarCampo(estado => ({
          ...estado,
          nome: true,
        }));
        buscandoDados(false);
      } catch (error) {
        if (mensagemErroConsultaRF) {
          erro(mensagemErroConsultaRF);
        } else {
          erros(error);
        }
        buscandoDados(false);
        setPessoaSelecionada({
          professorRf: '',
          professorNome: '',
          usuarioId: '',
        });
      }
    },
    [anoLetivo, buscarOutrosCargos, mensagemErroConsultaRF]
  );

  const onChangeRF = valor => {
    if (valor.length === 0) {
      setPessoaSelecionada({
        professorRf: '',
        professorNome: '',
        usuarioId: '',
      });
      setDesabilitarCampo(estado => ({
        ...estado,
        nome: false,
      }));
    }
  };

  const onSelectPessoa = objeto => {
    setPessoaSelecionada({
      professorRf: parseInt(objeto.key, 10),
      professorNome: objeto.props.value,
    });
    setDesabilitarCampo(estado => ({
      ...estado,
      rf: true,
    }));
  };

  useEffect(() => {
    if (rfEdicao && !pessoaSelecionada?.professorRf) {
      onBuscarPorRF({ rf: rfEdicao });
    } else if (!form && !rfEdicao) {
      setPessoaSelecionada({
        professorRf: '',
        professorNome: '',
        usuarioId: '',
      });
    }
  }, [rfEdicao]);

  useEffect(() => {
    onChange(pessoaSelecionada);
    if (form) {
      form.setValues({
        ...form.values,
        ...pessoaSelecionada,
      });
    }
  }, [pessoaSelecionada]);

  useEffect(() => {
    if (form) {
      if (validaSeObjetoEhNuloOuVazio(form.initialValues)) return;
      if (form.initialValues) {
        setPessoaSelecionada(form.initialValues);
      }
    }
  }, [form?.initialValues]);

  useEffect(() => {
    if (dreId && validacaoDesabilitaPerfilProfessor()) {
      onBuscarPorRF({ rf });
    }
  }, [dreId, ehPerfilProfessor, rf, onBuscarPorRF]);

  useEffect(() => {
    if (form) {
      const { values: valores } = form;
      if (valores && !valores.professorRf && pessoaSelecionada.professorRf) {
        setPessoaSelecionada({
          professorRf: '',
          professorNome: '',
          usuarioId: '',
        });
      }
    }
  }, [form?.values]);

  useEffect(() => {
    if (Object.keys(pessoaSelecionada).length && limparCamposAposPesquisa) {
      setPessoaSelecionada({});
      setDesabilitarCampo({
        rf: false,
        nome: false,
      });
    }
  }, [pessoaSelecionada, limparCamposAposPesquisa]);

  return (
    <>
      <Grid cols={4} className={classesRF}>
        {showLabel && <Label text={labelRF} control="professorRf" />}
        <InputRF
          pessoaSelecionada={pessoaSelecionada}
          onSelect={onBuscarPorRF}
          onChange={onChangeRF}
          name="professorRf"
          placeholderRF={placeholderRF}
          form={form}
          desabilitado={
            desabilitado ||
            validacaoDesabilitaPerfilProfessor() ||
            desabilitarCampo.rf
          }
          exibirLoader={exibirLoader}
        />
      </Grid>
      <Grid className="pr-0" cols={8}>
        {showLabel && <Label text={labelNome} control="professorNome" />}
        <InputNome
          dataSource={dataSource}
          onSelect={onSelectPessoa}
          onChange={onChangeInput}
          pessoaSelecionada={pessoaSelecionada}
          form={form}
          name="professorNome"
          placeholderNome={placeholderNome}
          desabilitado={
            desabilitado ||
            validacaoDesabilitaPerfilProfessor() ||
            desabilitarCampo.nome
          }
          exibirLoader={exibirLoader}
        />
      </Grid>
    </>
  );
}

Localizador.propTypes = {
  onChange: () => {},
  form: PropTypes.oneOfType([
    PropTypes.objectOf(PropTypes.object),
    PropTypes.any,
  ]),
  showLabel: PropTypes.bool,
  dreId: PropTypes.string,
  anoLetivo: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  desabilitado: PropTypes.bool,
  rfEdicao: PropTypes.string,
  buscarOutrosCargos: PropTypes.bool,
  buscandoDados: PropTypes.func,
  labelRF: PropTypes.string,
  labelNome: PropTypes.string,
  placeholderRF: PropTypes.string,
  placeholderNome: PropTypes.string,
  classesRF: PropTypes.string,
  limparCamposAposPesquisa: PropTypes.bool,
  validaPerfilProfessor: PropTypes.bool,
  mensagemErroConsultaRF: PropTypes.string,
};

Localizador.defaultProps = {
  onChange: PropTypes.func,
  form: null,
  showLabel: false,
  dreId: null,
  anoLetivo: null,
  desabilitado: false,
  rfEdicao: '',
  buscarOutrosCargos: false,
  buscandoDados: () => {},
  labelRF: 'Registro Funcional (RF)',
  labelNome: 'Nome',
  placeholderRF: 'Digite o RF',
  placeholderNome: 'Digite o nome da pessoa',
  classesRF: '',
  limparCamposAposPesquisa: false,
  validaPerfilProfessor: true,
  mensagemErroConsultaRF: '',
};

export default Localizador;
