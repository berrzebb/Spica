// svelte
import svelte from 'rollup-plugin-svelte';
// alias
import alias from '@rollup/plugin-alias';
import path from 'path';

import postcss from 'postcss';
import autoprefixer from 'autoprefixer';
import scss from 'rollup-plugin-scss';

import resolve from '@rollup/plugin-node-resolve';
import commonjs from '@rollup/plugin-commonjs';
import json from '@rollup/plugin-json';
import typescript from '@rollup/plugin-typescript';
import livereload from 'rollup-plugin-livereload';
import { terser } from 'rollup-plugin-terser';

const {isDev, buildDir, cssDir, appDir } = require('./configuration');

const plugins = [
    alias({
        entries: [
            {find: '@', replacement: path.resolve(__dirname, appDir) }
        ]
    }),
    svelte(require('./svelte.config')),
    scss({
        processor: css => postcss([autoprefixer])
        .process(css)
        .then(result => result.css)
    }),
    resolve({
        browser: true,
        dedupe: ['svelte']
    }),
    commonjs(),
    json(),
    typescript({
        inlineSources: isDev
    }),
];
if (isDev){
    plugins.push(
        serve(),
        livereload(buildDir)
    )
} else {
    plugins.push(
        terser({ sourcemap: isDev })
    )
}
export default {
	input: `${appDir}/main.ts`,
	output: {
		sourcemap: true,
		file: `${buildDir}/bundle.js`,
		format: 'iife',
		name: 'app',
    },
    plugins,
	watch: {
		clearScreen: false
	}
};

function serve() {
    let server;

    function toExit() {
        if(server) server.kill(0);
    }

    return {
		writeBundle(){
			if (server) return;
			server = require('child_process').spawn('/app/Spica', {
				stdio: ['ignore', 'inherit', 'inherit'],
				shell: true
			});
			process.on('SIGTERM', toExit);
			process.on('exit', toExit);
		}
    }
}