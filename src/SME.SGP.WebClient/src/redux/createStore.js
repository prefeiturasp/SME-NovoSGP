import { createStore, compose, applyMiddleware } from 'redux';

export default (reducers, middlewares) => {
  /* eslint-disable no-underscore-dangle */
  const enhancer =
    process.env.NODE_ENV === 'development'
      ? compose(console.tron.createEnhancer(), applyMiddleware(...middlewares))
      : applyMiddleware(...middlewares);
  /* eslint-enable */

  return createStore(reducers, enhancer);
  //
};
