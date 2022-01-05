const sveltePreprocess = require('svelte-preprocess')
const autoprefixer = require('autoprefixer')
const { isDev, appDir } = require('./configuration') 

module.exports = {
    preprocess: sveltePreprocess({
        sourceMap : isDev,
        scss:{
            prependData: `@import "${appDir}/assets/scss/variables.scss";`
        },
        postcss: {
            plugins: [autoprefixer()]
        }
    }),
    compilerOptions: {
        dev: isDev,
    },
}