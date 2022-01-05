const buildDir = 'wwwroot/build'
module.exports = {
    isDev : process.env.NODE_ENV === 'development',
    buildDir : buildDir,
    cssDir: `${buildDir}/css`,
    appDir: 'svelte-app'
}