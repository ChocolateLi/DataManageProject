import axios from 'axios'
import UserDto from '../class/UserDto'
//查询
export const getList = (req: UserDto) => {
    return axios.post("/api/list", req)
}
//新增
export const add = (req: {}) => {
    return axios.post("/api/add", req)
}
//修改
export const edit = (req: {}) => {
    return axios.post("/api/edit", req)
}
//删除
export const del = (ids: string) => {
    return axios.get("/api/del?ids=" + ids)
}
