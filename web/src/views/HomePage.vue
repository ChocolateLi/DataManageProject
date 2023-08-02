<template>
    <div class="container">
        <el-row>
            <el-col :span="12">
                <el-input v-model="SearchVal" placeholder="Please input" class="input-with-select"
                    @keyup.enter="enterSearch">
                    <template #append>
                        <el-button :icon="Search" @click="enterSearch" />
                    </template>
                </el-input>
            </el-col>
            <el-col :span="12">
                <el-button type="primary" @click="openAdd">add</el-button>
                <el-button type="danger" @click="onDel">delete</el-button>
            </el-col>
        </el-row>
        <el-table :data="tableData" style="width: 100%" ref="multipleTableRef">
            <el-table-column type="selection" width="55" />
            <el-table-column prop="date" label="Date" width="240" />
            <el-table-column prop="name" label="Name" width="150" />
            <el-table-column prop="order" label="Order" width="80" />
            <el-table-column prop="address" label="Address" />
            <el-table-column label="Operations">
                <template #default="scope">
                    <el-button size="small" @click="handleEdit(scope.$index, scope.row)">Edit</el-button>
                    <el-button size="small" type="danger" @click="handleDelete(scope.$index, scope.row)">Delete
                    </el-button>
                </template>
            </el-table-column>
        </el-table>
        <el-pagination background layout="prev, pager, next" :total="total" @current-change="currentChange" />
    </div>
    <addVue :isShow="isShow" :info="info" @closeAdd="closeAdd" @success="success"></addVue>
</template>
<script lang="ts" setup>
import { ref, onMounted } from 'vue'
import { Search } from '@element-plus/icons-vue'
import addVue from '../components/add.vue';
import User from '../class/User';
import UserDto from '../class/UserDto';
import { getList, del } from '../http';
const multipleTableRef = ref()
const tableData = ref<any[]>([])
let userDto = ref<UserDto>(new UserDto())
const total = ref(100)
const load = async () => {
    let data = (await getList(userDto.value)).data
    tableData.value = data.res
    total.value = data.total
}
const SearchVal = ref("")
onMounted(async () => {
    await load()
})
const currentChange = async (val: number) => {
    console.log(val)
    userDto.value.PageIndex = val
    await load()
}
const enterSearch = async () => {
    userDto.value.KeyWord = SearchVal.value
    await load()
}

const isShow = ref(false)
const openAdd = () => {
    isShow.value = true
}
const closeAdd = () => {
    isShow.value = false
    info.value = new User()
}
const success = async (message: string) => {
    isShow.value = false
    info.value = new User()
    ElMessage.success(message)
    await load()
}
const onDel = async () => {
    let rows = multipleTableRef.value?.getSelectionRows() as Array<User>
    if (rows.length > 0) {
        console.log(rows.map(item => { return `'${item.id}'` }).join(","))
        // 'a','b','c'
        let res = (await del(rows.map(item => { return `'${item.id}'` }).join(","))).data
        if (res) {
            ElMessage.success("删除成功！")
            await load()
        } else {
            ElMessage.error("删除失败！")
        }
    } else {
        ElMessage.warning("请选中需要删除的行！")
    }

}
const info = ref<User>(new User())
const handleEdit = (index: number, row: User) => {
    console.log(index, row)
    info.value = row
    isShow.value = true
}
const handleDelete = (index: number, row: User) => {
    console.log(index, row)
}
</script>
<style lang="scss" scoped>
.container {
    margin: 100px auto;
    width: 50%;
}

.table {
    margin: 10px 0;
}

.input-with-select {
    width: 380px;
    margin-right: 20px;
}
</style>