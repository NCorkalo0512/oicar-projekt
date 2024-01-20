const { createTransformer } = require('babel-jest');

const svgTransformer = createTransformer({
  presets: ['@babel/preset-env', '@babel/preset-react'],
});

module.exports = svgTransformer;