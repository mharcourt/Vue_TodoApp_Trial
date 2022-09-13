import request from './request'

export function getUser(userEmail, userName)
{
    return request({
        url: 'User/getUser',
        method: 'GET',
        params:{
            userEmail,
            userName
        }
    })
}

export function insertUser(userEmail, userName)
{
    return request({
        url: 'User/insertUser',
        method: 'POST',
        params:{
            userEmail,
            userName
        }
    })
}

export function insertItem(userId, content)
{
    return request({
        url: 'Todo/insertItem',
        method: 'POST',
        params:{
            userId,
            content
        }
    })
}

export function getItems(userId)
{
    return request({
        url: 'Todo/getItems',
        method: 'GET',
        params:{
            userId
        }
    })
}

export function updateItem(userId, rowKey, content, isDone)
{
    return request({
        url: 'Todo/updateItem',
        method: 'PUT',
        params:{
            userId,
            rowKey,
            content,
            isDone
        }
    })
}

export function deleteItem(userId, rowKey, content, isDone)
{
    return request({
        url: 'Todo/updateItem',
        method: 'DELETE',
        params:{
            userId,
            rowKey,
            content,
            isDone
        }
    })
}