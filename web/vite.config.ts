import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import AutoImport from 'unplugin-auto-import/vite'
import Components from 'unplugin-vue-components/vite'
import { ElementPlusResolver } from 'unplugin-vue-components/resolvers'
// https://vitejs.dev/config/
export default defineConfig({
  plugins: [
    vue(),
    AutoImport({
      resolvers: [ElementPlusResolver()],
    }),
    Components({
      resolvers: [ElementPlusResolver()],
    }),
  ],
  server: {
    open: true, //自动打开浏览器
    proxy:{
      '/api':{
        target:"http://localhost:5204",
        changeOrigin:true,
        rewrite(path){
          return path.replace(/^\/api/,'')
        }
      }
    }
  }
})
