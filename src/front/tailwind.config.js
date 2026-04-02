/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}",
  ],
  theme: {
    fontFamily: {
      'viga': ["Viga"],
      "gfsdidot": ["GFSDidot"],
      "montserrat": ["Montserrat"],
    },
    extend: {
      colors: {
        primary: '#6f0523',
        'primary-light': '#8a0a2e',
        'primary-dark': '#520418',
        gold: '#dabb7f',
        'gold-dark': '#b8954f',
        'gold-light': '#e8d4a8',
        secondary: '#1a3c34',
        'secondary-light': '#2d5a3f',
      },
    },
  },
  plugins: [
    require('@tailwindcss/aspect-ratio'),
  ],
}

