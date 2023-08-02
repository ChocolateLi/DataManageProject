<template>
    <el-dialog v-model="dialogVisible" :title="info?.name ? '修改' : '新增'" width="30%" @close="$emit('closeAdd')"
        draggable>
        <el-form :model="form" label-width="60px" ref="ruleFormRef" :rules="rules">
            <el-form-item label="时间" prop="date">
                <el-date-picker v-model="form.date" type="date" placeholder="请选择一个时间" :disabledDate="disablesDate" />
            </el-form-item>
            <el-form-item label="名称" prop="name">
                <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="地址" prop="address">
                <el-input v-model="form.address" />
            </el-form-item>
            <el-form-item label="排序" prop="order">
                <el-input v-model.number="form.order" />
            </el-form-item>
        </el-form>
        <template #footer>
            <span class="dialog-footer">
                <el-button @click="closeAdd(ruleFormRef)">Cancel</el-button>
                <el-button type="primary" @click="save(ruleFormRef)">Confirm</el-button>
            </span>
        </template>
    </el-dialog>
</template>
<script lang="ts" setup>
import { ref, reactive, Ref, computed, watch } from 'vue'
import User from '../class/User'
import { FormInstance, FormRules } from 'element-plus'
import { add,edit } from '../http';
const props = defineProps({
    isShow: Boolean,
    info: User
})
const dialogVisible = computed(() => props.isShow)
const ruleFormRef = ref<FormInstance>()
const form: Ref<User> = ref<User>({
    id: "",
    date: "",
    name: "",
    address: "",
    order: 0
})
watch(() => props.info, (newInfo) => {
    if (newInfo) {
        form.value = {
            id: newInfo.id,
            date: newInfo.date,
            name: newInfo.name,
            address: newInfo.address,
            order: newInfo.order
        }
    }
})
const rules = reactive<FormRules>({
    date: [{
        type: 'date',
        required: true,
        message: '请选择一个时间',
        trigger: 'change',
    }],
    name: [
        { required: true, message: '请输入名称', trigger: 'blur' }
    ],
    address: [
        { required: true, message: '请输入地址', trigger: 'blur' }
    ],
    order: [
        { required: true, message: '请输入一个序号' },
        { type: 'number', message: '该字段必须是数字' }
    ]
})
const disablesDate = (time: any) => {
    //最大时间，从今天开始
    const _maxTime = Date.now() - 24 * 60 * 60 * 1000 * 1
    return time.getTime() <= _maxTime
}
const emits = defineEmits(["closeAdd", "success"])
const closeAdd = async (formEl: FormInstance | undefined) => {
    if (!formEl) return
    formEl.resetFields()
    emits("closeAdd")
}
const save =  async (formEl: FormInstance | undefined) => {
    if (!formEl) return
    await formEl.validate((valid, fields) => {
         if (valid) {
             if (form.value.id) {
                 edit(form.value).then(function (res) {
                     if (res.data) {
                         emits("success", "修改成功！")
                     }
                 })
             } else {
                 add(form.value).then(function (res) {
                     if (res.data) {
                         emits("success", "添加成功！")
                     }
                 })
             }
         } else {
             console.log('error submit!', fields)
         }
     }) 
}
</script>