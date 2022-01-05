<script lang='ts'>
	export let name : string;
</script>

<main>
	<h1>Hi!! {name}!</h1>
	<p>Visit the <a href="https://svelte.dev/tutorial">Svelte tutorial</a> to learn how to build Svelte apps.</p>
</main>

<style lang="scss">
	h1 {
		color:$primary-color;
		text-transform: uppercase;
		font-size: 4em;
		font-weight: 100;
		user-select: none;
	}
	@media (min-width: 640px) {
		main {
			max-width: none;
		}
	}
</style>