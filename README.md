# DataManageProject

# 项目介绍

本项目是一个简单的数据管理平台，实现了基本的增删改查以及查询等功能。<br>

前端项目是通过Vue3框架搭建的，后端是使用.NET 6 的 Minimal api 搭建的。<br>

api文件夹是后端项目，web文件夹是前端项目<br>

其中技术栈包括如下：<br>

前端：Vue3、Vite、TypeScript、SCSS、Element Plus、Router、axios<br>

后端：.NET6、Minimal API、Sql sugar (CodeFirst)<br>

项目界面：

![](https://github.com/ChocolateLi/DataManageProject/blob/main/img/%E7%AE%80%E5%8D%95%E7%9A%84%E7%95%8C%E9%9D%A2%E7%AE%A1%E7%90%86.png)

# 项目启动

先启动后端再启动前端

## 后端项目启动

直接通过Visual Studio 2022启动

## 前端项目启动

win + R，输入cmd命令，进入终端，切换到 web 文件夹目录下，输入`pnpm dev` 命令进行启动。<br>

如果启动过程中出现以下错误：

![](https://github.com/ChocolateLi/DataManageProject/blob/main/img/MODULE_NOT_FOUND.png)

则把 web 目录下的 node_modules 文件夹删除，然后使用`pnpm install`命令进行重新安装。

安装完成后再使用`pnpm dev`命令进行项目的启动。

# 需要修改的配置文件

## 1.数据库文件

后端数据库文件需要配置你的服务器地址，修改 api 文件下的 Data 文件夹的 SqlSugarHelper.cs 文件

修改ConnectionString字符串，本项目使用的是SqlServer数据库

```c#
ConnectionString = "Data Source=电脑主机名;Initial Catalog=数据库名称;Persist Security Info=True;User ID=sa;Password=123456",//连接符字串
```

## 2.代理文件

前后端联调过程中，出现了跨域问题，因此需要解决跨域问题，本项目采用的方案是在前端项目中配置代理，从而解决浏览器跨域问题。

在 web 文件夹下，找到 vite.config.ts 文件。

其中 target需要修改为后端访问接口的地址。

```c#
server: {
    open: true, //自动打开浏览器
    proxy:{
      '/api':{
        target:"http://localhost:5204",//后端访问接口地址
        changeOrigin:true,
        rewrite(path){
          return path.replace(/^\/api/,'')
        }
      }
    }
  }
```



