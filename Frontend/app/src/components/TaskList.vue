<template>
    <div>
        <div v-if="openLogin" class="modalBox">
            <h1>Login</h1>
            <input placeholder="Email" type="text" v-model="EmailInput"/>
            <input placeholder="Name" type="text" v-model="NameInput"/>
            <button @click="handleGetUser()">Login</button>
            <button @click="openCreate = true, openLogin=false">Create User</button>
        </div>
    
        <div v-if="openCreate" class="modalBox">
            <h1>Create New User</h1> 
            <input placeholder="Email" v-model="EmailInput"/>
            <input placeholder="Name" v-model="NameInput"/>
            <button @click="handleInsertUser()">Create User</button>
            <button @click="openCreate = false, openLogin=true">Cancel</button>
        </div>

        <div v-if="openTasks" class="modalBox">
            <h1>To Do</h1>
            <button @click="openTasks=false, openNewItem=true, TaskInput = ''">Add New Item</button>
            <table>
                <tbody>
                    <tr>
                        <td>Task</td>
                        <td>Is Done</td>
                    </tr>
                    <tr v-for="task in FilteredTasks" :key="task.rowKey">
                        <td>{{task.content}}</td>
                        <td>
                            <input type="checkbox" v-model="task.isDone"/>
                            </td><td>
                            <button @click="handleDeleteTask(task)">Delete</button>
                            <button @click="editTaskOpen(task)">Edit</button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <button @click="handleLogout()">Logout</button>
        </div> 

        <div v-if="openEditItem" class="modalBox">
            <h1>Edit Item</h1> 
            <input type="text" v-model="TaskInput"/>
            <button @click="handleEditTask()">Apply</button>
            <button @click="openTasks = true, openEditItem=false">Cancel</button>
        </div>

        <div v-if="openNewItem" class="modalBox">
            <h1>Add New Item</h1> 
            <input placeholder="New Item" v-model="TaskInput"/>
            <button @click="handleAddTask()">Add</button>
            <button @click="openTasks = true, openNewItem=false">Cancel</button>
        </div>
    </div>
</template>

<script>
//import { insertUser, getUser, insertItem, getItems, updateItem, deleteItem} from '../utils/api'

export default{
    name: "TaskApp",
    data() {
        return {
            openCreate: false,
            openLogin: true,
            openTasks: false,
            openNewItem:false,
            openEditItem:false,
            EmailInput: "",
            NameInput: "",
            TaskInput: '',
            currentUser: null,
            editItem: null,
            UserList: [{
                partitionKey: "user=qwerty",
                userEmail: "qwerty",
                userName: "asdf"
            }],
            FilteredTasks: [{}],
            TaskList: [{
                partitionKey: "user=qwerty",
                rowKey: "0",
                content: "Do Dishes",
                isDone: false
            },{ 
                partitionKey: "user=qwerty",
                rowKey: "1",
                content: "Do Washing",
                isDone: true
            }],
        };
    },
   methods:{
        handleInsertUser(){
             if (this.EmailInput !== '' && this.NameInput !== ''){
                if(this.UserList.filter(
                (user) =>
                    user.userEmail === this.EmailInput) == null)
                {
                    this.UserList.push({
                        partitionKey: "user=" + this.NameInput,
                        userEmail: this.EmailInput,
                        userName: this.NameInput
                    })
                    //insertUser(this.EmailInput, this.NameInput)
                    alert("Account Created")
                    this.EmailInput = ""
                    this.NameInput = ""
                    this.openCreate = false, 
                    this.openLogin=true
                }
                else
                {
                     alert("Email Already in Use");
                }
            }
            else
            {
                    alert("Empty Fields");
            }
        },
        handleGetUser(){
            if (this.EmailInput !== '' && this.NameInput !== ''){
                /*getUser(this.EmailInput, this.NameInput).then(data => {
                    if (data !== null)
                    {
                        this.currentUser = data[0]
                        this.openCreate = false
                        this.openLogin=false
                        this.openTasks = true
                    }
                })*/
                this.currentUser = this.UserList.filter(
                    (user) =>
                        user.userName === this.NameInput &&
                        user.userEmail === this.EmailInput
                    )[0];
                if(this.currentUser != null)
                {
                    this.handleGetTasks()
                    this.openCreate = false
                    this.openLogin=false
                    this.openTasks = true
                }
                else {
                    alert("User Not Found")
                }
            }
            else
            {
                    alert("Empty Fields");
            }
        },
        handleGetTasks(){
            //this.TaskList = this.getItems(this.currentUser.partitionKey)
            this.FilteredTasks = this.TaskList.filter(
                        (task) =>
                        task.partitionKey == this.currentUser.partitionKey
                    )
        },
        editTaskOpen(task){
            this.TaskInput = task.content;
            this.openTasks=false, 
            this.openEditItem=true,
            this.editItem=task
        },
        handleEditTask(){
            this.editItem.content  = this.TaskInput,
            this.TaskList[this.editItem.rowKey] = this.editItem,
            //updateItem(this.editItem.partitionKey, this.editItem.rowKey, this.editItem.content, this.editItem.isDone )
            this.handleGetTasks()
            this.openTasks=true, 
            this.openEditItem=false
        },
        handleAddTask(){
            this.TaskList.push({
                    partitionKey: this.currentUser.partitionKey,
                    rowKey: this.TaskList.length, 
                    content: this.TaskInput, 
                    isDone: false
                    })
            //insertItem(this.currentUser.partitionKey, this.TaskInput)
            this.handleGetTasks()
            this.openTasks=true, 
            this.openNewItem=false
        },
        handleDeleteTask(deleteTaskl){
            //deleteItem(this.editItem.partitionKey, this.editItem.rowKey, this.editItem.content, this.editItem.isDone )
             this.TaskList = this.TaskList.filter(
                        (task) =>
                        task.rowKey != deleteTaskl.rowKey
                    )
            this.handleGetTasks();
        },
        handleLogout(){
            this.currentUser = null,
            this.openLogin=true, 
            this.openTasks=false
        },
    }
}
</script>

<style>
.modalBox {
    position: fixed;
    display: flex;
    flex-direction: column;
    align-items: center;
    top: 40%;
    left: 50%;
    width: 300px;
    margin-left: -150px;
    background-color: aqua;
    padding-bottom: 2%;
}
</style>