import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import path from 'path';

const root = path.join(__dirname, './');
const main = path.resolve(__dirname, 'src', 'main.tsx');

// https://vitejs.dev/config/
export default defineConfig(({ command }) => ({
  plugins: [react()],
  publicDir: command === 'build' ? false : 'src/assets',
   root: root,
  resolve: {
    extensions: ['.js', '.jsx', '.tsx', '.ts'],
  },
  server: {
    port: 3000
  },

  build: {
    outDir: './dist/umbraco-app',
    minify: false ,
    brotliSize: false,
    manifest: false,
    sourcemap: command === 'serve' ? 'inline' : false,
    rollupOptions: {
      input: {
        main: main
      },
      output: {
        assetFileNames: 'assets/[name][extname]',
        chunkFileNames: 'chunks/[name].[hash].js',
        entryFileNames: 'js/[name].js',
      },
    },
  },

}))
