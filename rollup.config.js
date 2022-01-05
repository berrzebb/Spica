import svelte from 'rollup-plugin-svelte-hot';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';
import livereload from 'rollup-plugin-livereload';
import { terser } from 'rollup-plugin-terser';
import typescript from '@rollup/plugin-typescript';
import json from '@rollup/plugin-json';
import sveltePreprocess from 'svelte-preprocess';
import scss from 'rollup-plugin-scss';
import sass from 'node-sass';

import hmr from 'rollup-plugin-hot';
import css from 'rollup-plugin-css-only';
import autoprefixer from 'autoprefixer';

const nollup = !!process.env.NOLLUP;
const watch = !!process.env.ROLLUP_WATCH;
const useLiveReload = !!process.env.LIVERELOAD;

const dev = watch || useLiveReload;
const production = !dev;

const hot = watch && !useLiveReload;
function serve() {
    let server;

    function toExit() {
        if(server) server.kill(0);
    }

    return {
		writeBundle(){
			if (server) return;
			server = require('child_process').spawn('dotnet',['Spica.dll'], {
				stdio: ['ignore', 'inherit', 'inherit'],
				shell: true,
				detached: true
			});
			process.on('SIGTERM', toExit);
			process.on('exit', toExit);
		}
    }
}
const sveltePreprocessing = sveltePreprocess({
	sourceMap : !production,
	sass: {
		sync: true,
		implementation : sass
	},
	postcss: {
		plugins: [autoprefixer()]
	}
});
export default {
	input: 'svelte-app/main.ts',
	output: {
		sourcemap: true,
		file: 'wwwroot/build/bundle.js',
		format: 'iife',
		name: 'app',
    },
	plugins: [
		svelte({
            preprocess: sveltePreprocessing,
			hot: hot && {
				optimistic: true,
				noPreserveState: false
			},
			emitCss: true,
			compilerOptions: {
				dev: !production,
			},
		}),
		css({
			output: 'css/bundle.css'
		}),
		scss({
			output: 'css/assets.css',
			failOnError: true,
			processor: css => postcss([autoprefixer])
		}),
		// If you have external dependencies installed from
		// npm, you'll most likely need these plugins. In
		// some cases you'll need additional configuration -
		// consult the documentation for details:
		// https://github.com/rollup/plugins/tree/master/packages/commonjs
		resolve({
			browser: true,
            dedupe: importee => importee === 'svelte' || importee.startsWith('svelte/')
		}),
		commonjs(),
		json(),
        typescript({
            sourceMap : !production,
            inlineSources: !production
        }),
        dev && !nollup && serve(),
		!production && livereload("svelte-app"),
		production && terser(),
		hmr({
			public: "svelte-app",
			inMemory: true,
			host: "0.0.0.0",
			compatModuleHot: !hot
		})
	],
	watch: {
		clearScreen: false
	}
};