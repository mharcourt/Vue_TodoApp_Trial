import axios from 'axios'

const service = axios.create({
  baseURL: 'https://localhost:7121/api/',
  timeout: 0
})

service.interceptors.response.use(
  response => {
    console.log(response.data)
    return response.data
  },
  error => {
    return Promise.reject(error)
  }
)

export default service