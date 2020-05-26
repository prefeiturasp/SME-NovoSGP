export const types = {
  salvarVersao: '@sistema/salvarVersao',
};

export const salvarVersao = payload => ({
  type: types.salvarVersao,
  payload,
});
