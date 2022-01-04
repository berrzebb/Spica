import svelte from 'rollup-plugin-svelte';
import commonjs from '@rollup/plugin-commonjs';
import resolve from '@rollup/plugin-node-resolve';
import livereload from 'rollup-plugin-livereload';
import { terser } from 'rollup-plugin-terser';
import typescript from '@rollup/plugin-typescript';
import json from '@rollup/plugin-json';
import css from 'rollup-plugin-css-only'
import sveltePreprocess from 'svelte-preprocess';
const production = !process.env.ROLLUP_WATCH;

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
export default {
	input: 'svelte-app/main.ts',
	output: {
		sourcemap: true,
		file: 'wwwroot/build/bundle.js',
		format: 'iife',
		name: 'app',
    },
	plugins: [
        json(),
		svelte({
            preprocess: sveltePreprocess({sourceMap : !production}),
            compilerOptions: {
                // enable run-time checks when not in production
                dev: !production
			}
		}),
        css({ output: 'bundle.css'}),

		// If you have external dependencies installed from
		// npm, you'll most likely need these plugins. In
		// some cases you'll need additional configuration -
		// consult the documentation for details:
		// https://github.com/rollup/plugins/tree/master/packages/commonjs
		resolve({
			browser: true,
            dedupe: ['svelte']
		}),
		commonjs(),
        typescript({
            sourceMap : !production,
            inlineSources: !production
        }),
        !production && serve(),
		!production && livereload('wwwroot'),
		production && terser()
	],
	watch: {
		clearScreen: false
	}
};